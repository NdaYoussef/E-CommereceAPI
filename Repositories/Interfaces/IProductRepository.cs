using TestToken.DTO;
using TestToken.DTO.ProductDto;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;

namespace TestToken.Repositories.Interfaces
{
    public interface IProductRepository:IGenericRepository<Product> 
    {
        Task<ResponseDto> GetAllProducts();
        Task<ResponseDto> GetProductById(int id);
        Task<ResponseDto> GetProductByName(string name);
        Task<ResponseDto> AddProduct(ProductDto product);
        Task<ResponseDto> UpdateProduct(int id , ProductDto productDto);
        Task<ResponseDto> DeleteProduct(int id);
        Task<ResponseDto> GetProductsByBrandName(string name);
        Task<ResponseDto> GetProductsByCategory(string categoryName);
    }
}
