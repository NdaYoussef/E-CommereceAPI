using System.ComponentModel.DataAnnotations.Schema;

namespace TestToken.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Method { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public double Amount { get; set; }
        public string CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual ApplicationUser Customer { get; set; }
    }
}
