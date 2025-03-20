using TestToken.Models;

namespace TestToken.DTO.CartDtos
{
    public class CartDto
    {
        public string CustomerId { get; set; }
       // public ICollection<CartItem>? CartItems { get; set; } = new List<CartItem>();
        public decimal? TotalPrice { get; set; }
    }
}
