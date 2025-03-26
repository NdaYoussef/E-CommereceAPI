using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestToken.DTO.OrderDto;
using TestToken.Models;
using TestToken.UOW;

namespace TestToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderItemController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("Items")]
        public async Task<IActionResult> GetAllOrderItems()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.OrderItems.GetAllItems();
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("ItemById/{id}")]
        public async Task<IActionResult> GetItem(int id )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.OrderItems.GetItemByID(id);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpPost("AddItem")]
        public async Task<IActionResult> AddItem(OrderItemDto orderItem)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.OrderItems.AddItem(orderItem);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        [Authorize(Policy = "Admin")]
        [HttpPut("UpdateItem")]
        public async Task<IActionResult> UpdateItem(int id ,OrderItemDto orderItem)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.OrderItems.UpdateItem(id,orderItem);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        [Authorize(Policy = "Admin")]
        [HttpPut("DeleteItem/{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.OrderItems.DeleteItem(id);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
    }
}
