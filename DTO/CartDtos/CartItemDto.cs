namespace TestToken.DTO.CartDtos
{
    public class CartItemDto
    {
        public int quantity { get; set; }
        public int? CartId { get; set; }
        public int? ProductId { get; set; }
    }
}
