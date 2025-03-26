using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestToken.DTO.CartDtos;
using TestToken.Models;
using TestToken.UOW;

namespace TestToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("Carts")]
        public async Task<IActionResult> GetCarts()
        {
            if (ModelState.IsValid)
            {
                var response = await _unitOfWork.Carts.GetAllCart();
                if (response.IsSucceeded)
                    return Ok(response);
                return StatusCode(response.StatusCode, response.Message);
            }
            return BadRequest(ModelState);

        }
        [Authorize(Policy = "Admin")]
        [HttpGet("CartById/{id}")]
        public async Task<IActionResult> GetCartById(int id)
        {
            if (ModelState.IsValid)
            {
                var response = await _unitOfWork.Carts.GetCart(id);
                if (response.IsSucceeded)
                    return Ok(response);
                return StatusCode(response.StatusCode, response.Message);
            }
            return BadRequest(ModelState);
        }
        [HttpPost("AddCart")]
        public async Task<IActionResult> AddCart(string customerId)
        {
            if (ModelState.IsValid)
            {
                var response = await _unitOfWork.Carts.AddCart(customerId);
                if (response.IsSucceeded)
                    return Ok(response);
                return StatusCode(response.StatusCode, response.Message);
            }
            return BadRequest(ModelState);
        }
        [HttpPost("AddDiscount")]
        public async Task<IActionResult> AddDiscount(int id , DiscountCodeDto discountCode)
        {
            if (ModelState.IsValid)
            {
                var response = await _unitOfWork.Carts.ApplyDiscountCode(id, discountCode);
                if (response.IsSucceeded)
                    return Ok(response);
                return StatusCode(response.StatusCode, response.Message);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("UpdateCart")]
        public async Task<IActionResult> UpdateCart(int id, CartDto cart)
        {
            if (ModelState.IsValid)
            {
                var response = await _unitOfWork.Carts.UpdateCart(id, cart);
                if (response.IsSucceeded)
                    return Ok(response);
                return StatusCode(response.StatusCode, response.Message);
            }
            return BadRequest(ModelState);
        }
        [Authorize(Policy = "Admin")]
        [HttpDelete("DeleteCart/{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            if (ModelState.IsValid)
            {
                var response = await _unitOfWork.Carts.DeleteCart(id);
                if (response.IsSucceeded)
                    return Ok(response);
                return StatusCode(response.StatusCode, response.Message);
            }
            return BadRequest(ModelState);
        }
    }
}
