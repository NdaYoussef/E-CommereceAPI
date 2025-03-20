using TestToken.DTO;
using TestToken.DTO.WishlistDto;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;

namespace TestToken.Repositories.Interfaces
{
    public interface IWishListRepository :IGenericRepository<WishList>
    {
        Task<ResponseDto> GetAllWishlistAsync();
        Task<ResponseDto> GetWishlistByIdAsync(int id);
        Task<ResponseDto> AddWishlistAsync(WishlistDto wishlistDto, string customerId);
        Task<ResponseDto> UpdateWishlistAsync(int id, WishlistDto wishlistDto);
        Task<ResponseDto> DeleteWishlistAsync(int id);
        Task<bool> ShareWishlistAsync(int id, string email);
    }
}
