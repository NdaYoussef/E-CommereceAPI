using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TestToken.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

  
        public int? CartId { get; set; }

        [JsonIgnore]
        [ForeignKey("CartId")]
        public Cart? Cart { get; set; }

        public int? ProductId { get; set; }
        [JsonIgnore]
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
    }
}
