using TestToken.DTO;
using TestToken.DTO.PaymentDto;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;

namespace TestToken.Repositories.Interfaces
{
    public interface IPaymentRepository:IGenericRepository<Payment>
    {
        Task<ResponseDto> CreatePayment(PaymentRequestDto paymentRequest);
        Task<ResponseDto> GetAllPayments();
    }
}
