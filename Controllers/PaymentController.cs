using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestToken.DTO.PaymentDto;
using TestToken.UOW;

namespace TestToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public PaymentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("Payments")]
        public async Task<IActionResult> GetAllPayments()
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Payments.GetAllPayments();
            if(response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode , response);
        }

        [HttpPost("Checkout")]
        public async Task<IActionResult> CreatePayment(PaymentRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Payments.CreatePayment(dto);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, response);
        }

    }
}
