using TestToken.DTO;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;

namespace TestToken.Repositories.Interfaces
{
    public interface ICategoryRepository:IGenericRepository<Category> 
    {
        Task<ResponseDto> GetAllCategories();
        Task<ResponseDto> GetCategoryById(int id);
        Task<ResponseDto> GetCategoryByName(string name);
        Task<ResponseDto> AddCategory(CategoryDto category);
        Task<ResponseDto> UpdateCategory(int id ,CategoryDto category);
        Task<ResponseDto> DeleteCategory(int id);
    }
}
