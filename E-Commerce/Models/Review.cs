using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestToken.Models
{
    public class Review
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int Rate { get; set; }

        [MaxLength(250)]
        public string Comment { get; set; }
        [JsonIgnore]
        public DateTime Date =>DateTime.UtcNow;

        public int? ProductId { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }
        [JsonIgnore]
        public string? CustomerId { get; set; }
        public ApplicationUser? Customer { get; set; }
    }
}
