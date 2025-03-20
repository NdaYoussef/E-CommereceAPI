using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TestToken.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int? CartId { get; set; }
        public int? ProductId { get; set; }

        [JsonIgnore]
        [ForeignKey("CartId")]
        public virtual Cart? Cart { get; set; }
        [JsonIgnore]
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}
