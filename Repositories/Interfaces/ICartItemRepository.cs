using TestToken.DTO;
using TestToken.DTO.CartDtos;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;

namespace TestToken.Repositories.Interfaces
{
    public interface ICartItemRepository :IGenericRepository<CartItem>
    {
        Task<ResponseDto> GetAllItems();
        Task<ResponseDto> GetItemById(int id);
        Task<ResponseDto> AddItem(CartItemDto item);   
        Task<ResponseDto> UpdateItem(int id , CartItem cartItem);
        Task<ResponseDto> DeleteItem(int id);
    }
}
