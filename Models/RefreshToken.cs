namespace TestToken.Models
{
    public class RefreshToken
    {
       public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsExpired =>DateTime.UtcNow > ExpiresOn;
        public bool IsActive =>! IsExpired || RevokedOn == null;
    }
}
