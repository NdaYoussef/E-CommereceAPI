namespace TestToken.DTO.ProductDto
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockAmount { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public string Image { get; set; }
        public string? categoryName { get; set; }
        public string? BrandName { get; set; }
        public int? categoryId { get; set; }
        public int? BrandId { get; set; }
    }
}
