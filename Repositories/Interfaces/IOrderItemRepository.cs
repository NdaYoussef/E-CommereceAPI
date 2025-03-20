using TestToken.DTO;
using TestToken.DTO.OrderDto;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;

namespace TestToken.Repositories.Interfaces
{
    public interface IOrderItemRepository:IGenericRepository<OrderItem>
    {
        Task<ResponseDto> GetItemByID(int id);
        Task<ResponseDto> GetAllItems();
        Task<ResponseDto> AddItem(OrderItemDto item);
        Task<ResponseDto> UpdateItem(int id , OrderItemDto item);
        Task<ResponseDto> DeleteItem(int id);
    }
}
