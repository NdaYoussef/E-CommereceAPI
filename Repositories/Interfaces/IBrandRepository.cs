using TestToken.DTO;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;

namespace TestToken.Repositories.Interfaces
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        public Task<ResponseDto> GetBrandById(int id);
        public Task<ResponseDto> GetBrandByName(string name);
        public Task<ResponseDto> GetAllBrands();
        public Task<ResponseDto> AddBrand(BrandDto brand);
        public Task<ResponseDto> UpdateBrand(int id , BrandDto brand);
        public Task<ResponseDto> DeleteBrand(int id);
    }
}
