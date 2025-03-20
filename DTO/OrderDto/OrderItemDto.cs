namespace TestToken.DTO.OrderDto
{
    public class OrderItemDto
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public double? Discount { get; set; }
        //public string ProductName { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }

    }
}
