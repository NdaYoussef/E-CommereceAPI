using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg.OpenPgp;
using TestToken.DTO.WishlistDto;
using TestToken.UOW;

namespace TestToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistItemController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public WishlistItemController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("AllItems")]
        public async Task<ActionResult> GetAllItems()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.WishListItems.GetAllWishlistItems();
            if(response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode , new {response.Message});
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("ItemById/{id}")]
        public async Task<ActionResult> GetItemById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.WishListItems.GetWishlistItemById(id);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("AddItem")]
        public async Task<ActionResult> AddItem(WishlistItemsDto wishlistItemsDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.WishListItems.AddItemtoWishlist(wishlistItemsDto);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode,response);
        }

        [Authorize(Policy = "Admin")]
        [HttpPut("EditItem")]
        public async Task<IActionResult> UpdatedItem(int id, WishlistItemsDto item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.WishListItems.UpdateWishlistItem(id, item);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpDelete("DeleteItem")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.WishListItems.DeleteWishlistItem(id);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
    }
}
