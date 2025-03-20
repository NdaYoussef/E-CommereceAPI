using System.ComponentModel.DataAnnotations;

namespace TestToken.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [EmailAddress]
        public string? Email { get; set; } = "no-email@example.com";
        public int? phone { get; set; }

        public ICollection<Product>? Products { get; set; }=new List<Product>();
    }

}
