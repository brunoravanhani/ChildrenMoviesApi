namespace ChildrenMoviesApi.Domain.Entity;

public class RefreshToken : EntityBase
{
    public string UserId { get; set; } = null!;
    public string Token { get; set; } = null!;
    public DateTime ExpiryDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? RevokedDate { get; set; }

    public bool IsActive => RevokedDate == null && ExpiryDate > DateTime.UtcNow;
}
