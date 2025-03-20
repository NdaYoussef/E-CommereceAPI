using TestToken.DTO;
using TestToken.DTO.OrderDto;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;

namespace TestToken.Repositories.Interfaces
{
    public interface IOrderRepository :IGenericRepository<Order>
    {
        Task<ResponseDto> GetAllOrders();
        Task<ResponseDto> GetOrederById(int id);
        Task<ResponseDto> CreateOrder(OrderDto order);
        Task<ResponseDto> UpdateOrder(int id , OrderDto order);
        Task<ResponseDto> DeleteOrder(int id);
    }
}
