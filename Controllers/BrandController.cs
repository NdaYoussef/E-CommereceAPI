using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestToken.DTO;
using TestToken.UOW;

namespace TestToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BrandController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("Brands")]
        public async Task<IActionResult> GetAllBrands()
        {
            if (ModelState.IsValid)
            {
                var response = await _unitOfWork.Brands.GetAllBrands();
                if(response.IsSucceeded)
                    return Ok(response);
                return StatusCode(response.StatusCode, response.Message);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("BrandById/{id}")]
        public async Task<IActionResult> GetBrandById(int id)
        {
            var response = await _unitOfWork.Brands.GetBrandById(id);

            if (ModelState.IsValid)
            {
                if (response.IsSucceeded)
                    return Ok(response);
            }
            return StatusCode(response.StatusCode,response.Message);
        }

        [HttpGet("BrandByName")]
        public async Task<IActionResult> GetBrandName([FromQuery]string name)
        {
            var response = await _unitOfWork.Brands.GetBrandByName(name);
            if (ModelState.IsValid)
            {
                if (response.IsSucceeded)
                    return Ok(response);
            }
            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpPost("AddBrand")]
        public async Task<IActionResult> AddBrand(BrandDto brandDto)
        {
            var response = await _unitOfWork.Brands.AddBrand(brandDto);
            if (ModelState.IsValid)
            {
                if (response.IsSucceeded)
                    return Ok(response);
            }
            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpPut("UpdateBrand")]
        public async Task<IActionResult> UpdateBrand(int id , [FromBody]BrandDto brandDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var brand = await _unitOfWork.Brands.UpdateBrand(id, brandDto);
            if(brand.IsSucceeded)
                return Ok(brand);
            return StatusCode(brand.StatusCode, brand.Message);
        }

        [HttpDelete("DeleteBrand/{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var brand = await _unitOfWork.Brands.DeleteBrand(id);
            if (brand.IsSucceeded)
                return Ok(brand);
            return StatusCode(brand.StatusCode, brand.Message);
        }
    }
}
