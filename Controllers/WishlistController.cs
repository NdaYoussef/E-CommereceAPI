using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestToken.DTO.WishlistDto;
using TestToken.UOW;

namespace TestToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public WishlistController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("Wishlists")]
        public async Task<IActionResult> GetAllWishlists()
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);
            var response = await _unitOfWork.WishLists.GetAllWishlistAsync();
            if(response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode , new {response.Message});

        }

        [Authorize(Policy = "Admin")]
        [HttpGet("Wishlist/{id}")]
        public async Task<IActionResult> GetWishListById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.WishLists.GetWishlistByIdAsync(id);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });

        }

        [HttpPost("AddWishlist")]
        public async Task<IActionResult> AddWishlist([FromBody]WishlistDto wishlistDto ,[FromQuery] string customerId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.WishLists.AddWishlistAsync(wishlistDto , customerId);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpPut("UpdateWishlist")]
        public async Task<IActionResult> UpdateWishlist(int id ,WishlistDto wishlistDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.WishLists.UpdateWishlistAsync(id , wishlistDto);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpDelete("DeleteWishlist")]
        public async Task<IActionResult> DeleteWishlist(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.WishLists.DeleteWishlistAsync(id);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpPost("ShareWishlist/{id}")]
        public async Task<IActionResult> ShareWishlist(int id , [FromQuery]string email)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.WishLists.ShareWishlistAsync (id , email);
            
            return response ? Ok("Wishlist shared successfully.") : BadRequest("Failed to share wishlist!");
        }
    }
}
