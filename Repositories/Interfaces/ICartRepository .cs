using TestToken.DTO;
using TestToken.DTO.CartDtos;
using TestToken.DTO.UserDtos;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;

namespace TestToken.Repositories.Interfaces
{
    public interface ICartRepository: IGenericRepository<Cart>
    {
        Task<ResponseDto> GetAllCart();
        Task<ResponseDto> GetCart(int id);
        Task<ResponseDto> AddCart(string customerId);
        Task<ResponseDto> UpdateCart(int id, CartDto cart);
        Task<ResponseDto> ApplyDiscountCode(int id, DiscountCodeDto discountPercentage);
        Task<ResponseDto> DeleteCart(int id);

    }
}
