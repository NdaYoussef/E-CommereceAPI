using AutoMapper;
using Microsoft.Extensions.Options;
using Stripe;
using TestToken.Data;
using TestToken.DTO;
using TestToken.DTO.PaymentDto;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;
using TestToken.Repositories.Interfaces;

namespace TestToken.Repositories.Services
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        public PaymentRepository(ApplicationDbContext context , IMapper mapper , IOptions<StripeSettings> options) :base(context)
        {
            _mapper = mapper;
        }
        public async Task<ResponseDto> CreatePayment(PaymentRequestDto paymentRequest)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = paymentRequest.Amount * 100, // Convert to cents
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" },
            };

            var service = new PaymentIntentService();
            var intent = await service.CreateAsync(options);

            return new ResponseDto
            {
                Message = "Payment intent created successfully",
                IsSucceeded = true,
                model = new { ClientSecret = intent.ClientSecret }
            };
        }
        

        public async Task<ResponseDto> GetAllPayments()
        {
            var service = new PaymentIntentService();
            var paymentIntents = await service.ListAsync(new PaymentIntentListOptions { Limit = 10 });

            return new ResponseDto
            {
                IsSucceeded = true,
                Message = "Payments retrieved successfully",
                model = paymentIntents.Data
            };
        }
    }
}
