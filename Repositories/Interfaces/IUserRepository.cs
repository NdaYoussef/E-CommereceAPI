using TestToken.DTO;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;

namespace TestToken.Repositories.Interfaces
{
    public interface IUserRepository :IGenericRepository<ApplicationUser>
    {
        Task<ResponseDto> GetAllUsersAsync();
    }
}
