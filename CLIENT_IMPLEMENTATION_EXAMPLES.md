
// ============================================================
// JAVASCRIPT/REACT EXAMPLE
// ============================================================

import axios from 'axios';

class AuthService {
  constructor(apiUrl = 'http://localhost:5000/api/auth') {
    this.apiUrl = apiUrl;
    this.accessToken = localStorage.getItem('accessToken');
    this.refreshToken = localStorage.getItem('refreshToken');
    
    // Setup axios interceptor for automatic token refresh
    this.setupInterceptor();
  }

  setupInterceptor() {
    axios.interceptors.response.use(
      response => response,
      error => {
        if (error.response?.status === 401) {
          return this.refreshAccessToken()
            .then(response => {
              // Retry original request with new token
              return axios.request(error.config);
            })
            .catch(() => {
              this.logout();
              window.location.href = '/login';
              return Promise.reject(error);
            });
        }
        return Promise.reject(error);
      }
    );
  }

  async googleLogin(idToken) {
    try {
      const response = await axios.post(`${this.apiUrl}/google`, { idToken });
      this.setTokens(response.data.token, response.data.refreshToken);
      return response.data;
    } catch (error) {
      console.error('Google login failed:', error);
      throw error;
    }
  }

  async refreshAccessToken() {
    try {
      const response = await axios.post(`${this.apiUrl}/refresh`, {
        refreshToken: this.refreshToken
      });
      this.setTokens(response.data.token, response.data.refreshToken);
      return response.data;
    } catch (error) {
      console.error('Token refresh failed:', error);
      this.clearTokens();
      throw error;
    }
  }

  async logout() {
    try {
      await axios.post(
        `${this.apiUrl}/logout`,
        { refreshToken: this.refreshToken },
        {
          headers: { Authorization: `Bearer ${this.accessToken}` }
        }
      );
    } finally {
      this.clearTokens();
    }
  }

  setTokens(accessToken, refreshToken) {
    this.accessToken = accessToken;
    this.refreshToken = refreshToken;
    localStorage.setItem('accessToken', accessToken);
    localStorage.setItem('refreshToken', refreshToken);
    
    // Set default auth header
    axios.defaults.headers.common['Authorization'] = `Bearer ${accessToken}`;
  }

  clearTokens() {
    this.accessToken = null;
    this.refreshToken = null;
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    delete axios.defaults.headers.common['Authorization'];
  }

  getAccessToken() {
    return this.accessToken;
  }
}

export default AuthService;


// ============================================================
// CURL EXAMPLES FOR TESTING
// ============================================================

# 1. Google Login
curl -X POST http://localhost:5000/api/auth/google \
  -H "Content-Type: application/json" \
  -d '{
    "idToken": "your_google_id_token_here"
  }'

Response:
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "L2t0SG9XQ0ZWdTJkamxCUHAxa3kxN3J1NFk1d3MrVWJZbWh4bzBsY0Z2UkxxN1lLYkNGSHRTTWJ6Vmp3ZQ=="
}


# 2. Refresh Token
curl -X POST http://localhost:5000/api/auth/refresh \
  -H "Content-Type: application/json" \
  -d '{
    "refreshToken": "L2t0SG9XQ0ZWdTJkamxCUHAxa3kxN3J1NFk1d3MrVWJZbWh4bzBsY0Z2UkxxN1lLYkNGSHRTTWJ6Vmp3ZQ=="
  }'

Response:
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "new_refresh_token_here"
}


# 3. Make Authenticated Request
curl -X GET http://localhost:5000/api/movies \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."

Response:
[
  { "id": 1, "title": "Movie Name", ... }
]


# 4. Logout
curl -X POST http://localhost:5000/api/auth/logout \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." \
  -H "Content-Type: application/json" \
  -d '{
    "refreshToken": "L2t0SG9XQ0ZWdTJkamxCUHAxa3kxN3J1NFk1d3MrVWJZbWh4bzBsY0Z2UkxxN1lLYkNGSHRTTWJ6Vmp3ZQ=="
  }'

Response:
{
  "message": "Logged out successfully"
}
