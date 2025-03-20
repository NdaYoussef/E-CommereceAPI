using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestToken.DTO.OrderDto;
using TestToken.UOW;

namespace TestToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("Orders")]
        public async Task<IActionResult> GetOrders()
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);
            var response = await _unitOfWork.Orders.GetAllOrders();
            if(response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode , new {response.Message});
        }

        [HttpGet("OrderById/{id}")]
        public async Task<IActionResult> OrderById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Orders.GetOrederById(id);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody]OrderDto orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Orders.CreateOrder(orderDto);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
        [HttpPut("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(int id ,OrderDto orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Orders.UpdateOrder(id, orderDto);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpDelete("DeleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Orders.DeleteOrder(id);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
    }
}
