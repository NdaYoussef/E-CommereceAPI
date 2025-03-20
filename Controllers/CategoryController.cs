using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestToken.DTO;
using TestToken.UOW;

namespace TestToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("Categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Categories.GetAllCategories();
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });

        }

        [HttpGet("CategoryById/{id}")]
        public async Task<IActionResult> categoryById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Categories.GetCategoryById(id);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpGet("CategoryName")]
        public async Task<IActionResult> CategoryName([FromQuery]string name )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Categories.GetCategoryByName(name);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory(CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Categories.AddCategory(categoryDto);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdatedCategory(int id ,CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Categories.UpdateCategory(id, categoryDto);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
        [HttpPut("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _unitOfWork.Categories.DeleteCategory(id);
            if (response.IsSucceeded)
                return Ok(response);
            return StatusCode(response.StatusCode, new { response.Message });
        }
    }
}
