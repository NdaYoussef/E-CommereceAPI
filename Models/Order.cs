using System.Text.Json.Serialization;

namespace TestToken.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date => DateTime.UtcNow;
        public string Status { get; set; }
        public int TrackingCode { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingMethod { get; set; }
        public double ShippingCost { get; set; }
        public string CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
