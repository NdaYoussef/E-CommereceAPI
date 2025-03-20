namespace TestToken.DTO.OrderDto
{
    public class OrderDto
    {
       // public int OrderId { get; set; }
        public int? TrackingCode { get; set; }
        public string Status { get; set; }
        public string? ShippingAddress { get; set; }
        public string? ShippingMethod { get; set; }
        public double? ShippingCost { get; set; }
        public string CustomerId { get; set; }

    }
}
