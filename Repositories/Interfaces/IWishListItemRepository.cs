using TestToken.DTO;
using TestToken.DTO.WishlistDto;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;

namespace TestToken.Repositories.Interfaces
{
    public interface IWishListItemRepository :IGenericRepository<WishListItem>
    {
        Task<ResponseDto> GetAllWishlistItems();
        Task<ResponseDto> GetWishlistItemById(int id);
        Task<ResponseDto> AddItemtoWishlist(WishlistItemsDto wishlistItemsDto);
        Task<ResponseDto> UpdateWishlistItem(int id , WishlistItemsDto item);
        Task<ResponseDto> DeleteWishlistItem(int id);
        Task<ResponseDto> GetItemInWishList(int listId);
    }
}
