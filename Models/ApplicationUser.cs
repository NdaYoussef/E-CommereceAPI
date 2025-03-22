using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TestToken.Models
{
    public class ApplicationUser:IdentityUser
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Address { get; set; }
        public string? OTP {  get; set; }
        public DateTime? OTPExpiry { get; set; } = DateTime.UtcNow;
        public bool IsConfirmed { get; set; } = false;
      
        public ICollection<Order>? Orders { get; set; }= new List<Order>();
        public ICollection<Review>? Reviews { get; set; }=new List<Review>();
        public ICollection<WishList>? Wishlists { get; set; } = new List<WishList>();
        public virtual ICollection<Cart>? Carts { get; set; } = new List<Cart>();
        public virtual ICollection<Payment>? Payments { get; set; } = new List<Payment>();
        public ICollection<RefreshToken>? refreshTokens = new List<RefreshToken>();

    }
}
