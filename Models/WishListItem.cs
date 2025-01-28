namespace TestToken.Models
{
    public class WishListItem
    {
        public int Id { get; set; }

        public int WishlistId { get; set; }
        public WishList? Wishlist { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
