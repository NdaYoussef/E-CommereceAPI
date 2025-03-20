using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestToken.DTO
{
    public class ReviewDto
    {
        public int Rate { get; set; }

        [MaxLength(250)]
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
     
        public int? ProductId { get; set; }
        public required string CustomerId { get; set; }

    }
}
