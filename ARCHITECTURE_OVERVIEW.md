# Refresh Token Strategy - Architecture Overview

## Complete System Architecture

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                           CLIENT APPLICATION                               │
│                                                                             │
│  ┌──────────────────────────────────────────────────────────────────────┐  │
│  │ Frontend (React/Angular/Vue)                                       │  │
│  │                                                                    │  │
│  │  1. User authenticates with Google OAuth                         │  │
│  │  2. Receives: accessToken (in memory) + refreshToken (storage)  │  │
│  │  3. Sends requests with Authorization: Bearer {accessToken}     │  │
│  │  4. On 401: Automatically refresh token                         │  │
│  │  5. On logout: Clear tokens from storage                        │  │
│  └──────────────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────────────────┘
                                    ↕ HTTP
┌─────────────────────────────────────────────────────────────────────────────┐
│                            API LAYER                                        │
│                  (src\ChildrenMoviesApi.Api)                               │
│                                                                             │
│  ┌──────────────────────────────────────────────────────────────────────┐  │
│  │ AuthController                                                     │  │
│  │                                                                    │  │
│  │  POST /auth/google          ← Google Login Request               │  │
│  │  POST /auth/refresh         ← Token Refresh Request              │  │
│  │  POST /auth/logout          ← Logout Request                     │  │
│  │                                                                    │  │
│  │  [Authorization middleware for protected endpoints]              │  │
│  └──────────────────────────────────────────────────────────────────┘  │
│                                    ↕                                      │
│  ┌──────────────────────────────────────────────────────────────────────┐  │
│  │ Models & DTOs                                                     │  │
│  │  - GoogleLoginDto (input)                                        │  │
│  │  - RefreshTokenDto (input)                                       │  │
│  │  - AuthTokenResponseDto (output)                                 │  │
│  └──────────────────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────────────────┘
                                    ↕
┌─────────────────────────────────────────────────────────────────────────────┐
│                        APPLICATION LAYER                                    │
│              (src\ChildrenMoviesApi.Application)                           │
│                                                                             │
│  ┌──────────────────────────────────────────────────────────────────────┐  │
│  │ IAuthService / AuthService                                        │  │
│  │                                                                    │  │
│  │  Methods:                                                          │  │
│  │  ├─ GoogleLoginAsync(idToken)                                     │  │
│  │  │  └─ Returns (AccessToken, RefreshToken)                       │  │
│  │  │                                                                │  │
│  │  ├─ RefreshTokenAsync(refreshToken)                              │  │
│  │  │  └─ Returns (NewAccessToken, NewRefreshToken)                 │  │
│  │  │                                                                │  │
│  │  └─ RevokeRefreshTokenAsync(refreshToken)                        │  │
│  │     └─ Marks token as revoked                                    │  │
│  └──────────────────────────────────────────────────────────────────┘  │
│                                    ↕                                      │
│  ┌──────────────────────────────────────────────────────────────────────┐  │
│  │ ITokenService / TokenService                                     │  │
│  │                                                                    │  │
│  │  Methods:                                                          │  │
│  │  ├─ GenerateToken(user)                                           │  │
│  │  │  └─ Creates JWT (2-hour expiry)                                │  │
│  │  │     Claims: NameIdentifier, Name, Email                       │  │
│  │  │                                                                │  │
│  │  └─ GenerateRefreshToken()                                        │  │
│  │     └─ Creates random base64 token (64 bytes)                     │  │
│  └──────────────────────────────────────────────────────────────────┘  │
│                                    ↕                                      │
│  ┌──────────────────────────────────────────────────────────────────────┐  │
│  │ IGoogleAuthService (via Dependency Injection)                   │  │
│  │                                                                    │  │
│  │  ├─ ValidateTokenAsync(idToken)                                   │  │
│  │  │  └─ Validates with Google, returns GoogleUserInfo             │  │
│  │  │                                                                │  │
│  │  └─ GetUserInfoAsync(userId)                                      │  │
│  │     └─ Retrieves user info for token refresh                     │  │
│  └──────────────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────────────────┘
                                    ↕
┌─────────────────────────────────────────────────────────────────────────────┐
│                         DOMAIN LAYER                                        │
│              (src\ChildrenMoviesApi.Domain)                                │
│                                                                             │
│  ┌──────────────────────────────────────────────────────────────────────┐  │
│  │ RefreshToken Entity                                               │  │
│  │                                                                    │  │
│  │  Properties:                                                       │  │
│  │  ├─ Id (GUID)                                                      │  │
│  │  ├─ UserId (string)                                                │  │
│  │  ├─ Token (string) - base64 random token                           │  │
│  │  ├─ ExpiryDate (DateTime) - 7 days from creation                  │  │
│  │  ├─ CreatedDate (DateTime)                                         │  │
│  │  ├─ RevokedDate (DateTime?) - null if active                       │  │
│  │  └─ IsActive (computed) - !Revoked && !Expired                     │  │
│  │                                                                    │  │
│  │  Relationships:                                                    │  │
│  │  └─ One RefreshToken per authentication session                   │  │
│  └──────────────────────────────────────────────────────────────────┘  │
│                                    ↕                                      │
│  ┌──────────────────────────────────────────────────────────────────────┐  │
│  │ IRefreshTokenRepository (Interface)                              │  │
│  │                                                                    │  │
│  │  Methods:                                                          │  │
│  │  ├─ AddAsync(refreshToken)                                        │  │
│  │  ├─ GetValidTokenAsync(token)                                     │  │
│  │  ├─ GetByUserIdAsync(userId)                                      │  │
│  │  ├─ RevokeAsync(token)                                            │  │
│  │  └─ SaveChangesAsync()                                            │  │
│  └──────────────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────────────────┘
                                    ↕
┌─────────────────────────────────────────────────────────────────────────────┐
│                     INFRASTRUCTURE LAYER                                    │
│              (src\ChildrenMoviesApi.Infra.MySQL)                           │
│                                                                             │
│  ┌──────────────────────────────────────────────────────────────────────┐  │
│  │ RefreshTokenRepository (Implementation)                           │  │
│  │                                                                    │  │
│  │  Uses: ChildrenMoviesDbContext                                     │  │
│  │  Query Logic:                                                      │  │
│  │  - Check token exists                                              │  │
│  │  - Verify not revoked (RevokedDate IS NULL)                       │  │
│  │  - Verify not expired (ExpiryDate > NOW)                          │  │
│  │  - Use indexes for fast lookup                                     │  │
│  └──────────────────────────────────────────────────────────────────┘  │
│                                    ↕                                      │
│  ┌──────────────────────────────────────────────────────────────────────┐  │
│  │ ChildrenMoviesDbContext (EF Core)                                │  │
│  │                                                                    │  │
│  │  DbSets:                                                           │  │
│  │  ├─ DbSet<Movie>                                                   │  │
│  │  ├─ DbSet<UserMovie>                                               │  │
│  │  └─ DbSet<RefreshToken> ← NEW                                      │  │
│  │                                                                    │  │
│  │  Configuration:                                                    │  │
│  │  └─ ConfigureRefreshTokenEntity()                                 │  │
│  │     ├─ Primary Key: Id                                            │  │
│  │     ├─ Unique Index: Token                                        │  │
│  │     └─ Index: UserId                                              │  │
│  └──────────────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────────────────┘
                                    ↕
┌─────────────────────────────────────────────────────────────────────────────┐
│                        DATABASE LAYER (MySQL)                              │
│                                                                             │
│  ┌──────────────────────────────────────────────────────────────────────┐  │
│  │ RefreshTokens Table                                               │  │
│  │                                                                    │  │
│  │  Columns:                                                          │  │
│  │  ├─ Id (CHAR(36)) - Primary Key                                    │  │
│  │  ├─ UserId (VARCHAR(100)) - indexed                                │  │
│  │  ├─ Token (LONGTEXT) - unique index                                │  │
│  │  ├─ ExpiryDate (DATETIME)                                          │  │
│  │  ├─ CreatedDate (DATETIME)                                         │  │
│  │  └─ RevokedDate (DATETIME, nullable)                               │  │
│  │                                                                    │  │
│  │  Indexes:                                                          │  │
│  │  ├─ PRIMARY KEY (Id)                                               │  │
│  │  ├─ UNIQUE KEY (Token) ← Fast token lookup                         │  │
│  │  └─ KEY (UserId) ← Find user's tokens                              │  │
│  └──────────────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Data Flow Diagram

### Login Flow
```
Client                    API                    Service              Repository             Database
  │                       │                       │                    │                       │
  ├─ POST /auth/google ─>│                       │                    │                       │
  │     {idToken}        │                       │                    │                       │
  │                       ├─ GoogleLoginAsync ──>│                    │                       │
  │                       │                       ├─ ValidateToken ──>│  (Google API)         │
  │                       │                       │   Returns User    │                       │
  │                       │                       ├─ GenerateToken ──────────────────────────│
  │                       │                       │   (JWT, 2h)       │                       │
  │                       │                       ├─ GenerateRefresh ─────────────────────────│
  │                       │                       │   (Base64)        │                       │
  │                       │                       ├─ AddAsync ───────>│ INSERT RefreshToken  │
  │                       │                       │                   ├─ SaveChanges ───────>│
  │                       │                       │                   │                  OK   │
  │                       │<─ Return Tokens ────<│                    │                       │
  │<─ {token, refresh} ──│                       │                    │                       │
```

### Refresh Flow
```
Client                    API                    Service              Repository             Database
  │                       │                       │                    │                       │
  ├─ POST /auth/refresh ->│                       │                    │                       │
  │   {refreshToken}      │                       │                    │                       │
  │                       ├─ RefreshTokenAsync ─>│                    │                       │
  │                       │                       ├─ GetValidToken ───>│ SELECT * WHERE       │
  │                       │                       │                   │ Token = X AND       │
  │                       │                       │                   │ RevokedDate IS NULL │
  │                       │                       │                   │ AND ExpiryDate >    │
  │                       │                       │<─ RefreshToken ────│ NOW()               │
  │                       │                       ├─ GenerateToken ──────────────────────────│
  │                       │                       │   (new JWT)       │                       │
  │                       │                       ├─ GenerateRefresh ─────────────────────────│
  │                       │                       │   (new Base64)    │                       │
  │                       │                       ├─ RevokeAsync ────>│ UPDATE RefreshToken │
  │                       │                       │   (old token)     │ SET RevokedDate=NOW │
  │                       │                       ├─ AddAsync ───────>│ INSERT new token    │
  │                       │                       │                   ├─ SaveChanges ───────>│
  │                       │<─ New Tokens ────────<│                    │                  OK   │
  │<─ {new token, new} ──│                       │                    │                       │
```

## Dependency Injection Flow

```
Program.cs
  │
  ├─ builder.Services.ApplicationDI()
  │  ├─ AddScoped<IAuthService, AuthService>
  │  └─ AddScoped<ITokenService, TokenService>
  │
  ├─ builder.Services.AddInfrastructure()
  │  ├─ AddDbContext<ChildrenMoviesDbContext>
  │  ├─ AddScoped<IMovieRepository, MovieRepository>
  │  ├─ AddScoped<IUserMovieRepository, UserMovieRepository>
  │  └─ AddScoped<IRefreshTokenRepository, RefreshTokenRepository>
  │
  └─ builder.Services.GoogleAuthDI()
     └─ AddScoped<IGoogleAuthService, GoogleAuthService>

When AuthController is instantiated:
  ├─ AuthService (requires:)
  │  ├─ IGoogleAuthService ← Provided by GoogleAuthDI
  │  ├─ ITokenService ← Provided by ApplicationDI
  │  └─ IRefreshTokenRepository ← Provided by AddInfrastructure
  │
  └─ RefreshTokenRepository (requires:)
     └─ ChildrenMoviesDbContext ← Provided by AddDbContext
```

## Security Layers

```
┌────────────────────────────────────────────────────────────────────┐
│ LAYER 1: Token Generation                                          │
│ ├─ AccessToken: Signed JWT with 2-hour expiry                     │
│ └─ RefreshToken: 64-byte cryptographically random base64 string   │
└────────────────────────────────────────────────────────────────────┘
              │
              ↓
┌────────────────────────────────────────────────────────────────────┐
│ LAYER 2: Token Storage                                             │
│ ├─ AccessToken: In-memory (volatile)                              │
│ └─ RefreshToken: Database + Client secure storage                 │
└────────────────────────────────────────────────────────────────────┘
              │
              ↓
┌────────────────────────────────────────────────────────────────────┐
│ LAYER 3: Token Validation                                          │
│ ├─ Check token exists in database                                 │
│ ├─ Check not revoked (RevokedDate IS NULL)                        │
│ └─ Check not expired (ExpiryDate > NOW)                           │
└────────────────────────────────────────────────────────────────────┘
              │
              ↓
┌────────────────────────────────────────────────────────────────────┐
│ LAYER 4: Token Rotation                                            │
│ ├─ Each refresh creates NEW tokens                                │
│ ├─ Old refresh token immediately revoked                          │
│ └─ Prevents token reuse if compromised                            │
└────────────────────────────────────────────────────────────────────┘
              │
              ↓
┌────────────────────────────────────────────────────────────────────┐
│ LAYER 5: Token Revocation                                          │
│ ├─ Logout immediately revokes token                               │
│ ├─ Revoked tokens never reactivate                                │
│ └─ Timestamp recorded for audit trail                             │
└────────────────────────────────────────────────────────────────────┘
```

## Summary

The refresh token strategy provides:
✅ Secure, separate short-lived and long-lived tokens
✅ Database-backed token management and revocation
✅ Automatic token rotation on refresh
✅ Clean API endpoints for login, refresh, and logout
✅ Integration with existing Google authentication
✅ Production-ready implementation
