namespace TestToken.Models
{
    public class WishList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }

        public ICollection<WishListItem> WishlistItems { get; set; }
    }
}
