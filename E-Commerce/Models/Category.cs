namespace TestToken.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CategoryId { get; set; }

        public ICollection<Product> Products { get; set; }=new List<Product>();
    }
}
