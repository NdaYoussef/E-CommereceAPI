using System.Text.Json.Serialization;

namespace TestToken.Models
{
    public class RefreshToken
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsExpired => DateTime.UtcNow > ExpiresOn;
        public bool IsActive => !IsExpired || RevokedOn == null;
        public string UserId { get; set; } 
        public ApplicationUser User { get; set; }
    }
}
