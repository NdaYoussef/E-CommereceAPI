using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestToken.Data;
using TestToken.DTO;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;
using TestToken.Repositories.Interfaces;

namespace TestToken.Repositories.Services
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public BrandRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager,IMapper mapper) : base(context)
        {
            _usermanager = userManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDto> AddBrand(BrandDto brand)
        {
            if(brand == null)
            {
                return new ResponseDto
                {
                    Message = "Brand data cannot be null",
                    IsSucceeded = false,
                    StatusCode = 400
                };
            }
            var newbrand = _mapper.Map<Brand>(brand);
            await _context.AddAsync(newbrand);
            await _context.SaveChangesAsync();
            var brandDto = _mapper.Map<BrandDto>(newbrand);
            return new ResponseDto
            {
                Message = "Brand added successfull",
                IsSucceeded = true,
                StatusCode = 201,
                model = brandDto
            };
        }

        public async Task<ResponseDto> DeleteBrand(int id)
        {
            var existingBrand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);
            if (existingBrand == null)
            {
                return new ResponseDto
                {
                    Message = "brand not found!!",
                    IsSucceeded = false,
                    StatusCode = 400
                };
            }
              _context.Brands.Remove(existingBrand);
            await _context.SaveChangesAsync();
            return new ResponseDto
            {
                Message = "Brand deleted successfully",
                IsSucceeded = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> GetAllBrands()
        {
            List<Brand> brands = await _context.Brands.AsNoTracking().ToListAsync();
            if(!brands.Any())
            {
                return new ResponseDto
                {
                    Message = "Invalid request data!",
                    IsSucceeded = false,
                    StatusCode = 400,
                    model = new List<Brand>()
                };
            }
            var brandsDto = _mapper.Map<List<BrandDto>>(brands);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                model = brandsDto
            };
        }

        public async Task<ResponseDto> GetBrandById(int id)
        {
          var existingBrand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);
            if (existingBrand == null)
            {
                return new ResponseDto
                {
                    Message = "Brand not found !",
                    StatusCode = 404,
                    IsSucceeded = false,
                };
            }
            var brandDto = _mapper.Map<BrandDto>(existingBrand);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                model = brandDto
            };

        }

        public async Task<ResponseDto> GetBrandByName(string name)
        {
            var existingBrand = await _context.Brands.FirstOrDefaultAsync(b => b.Name == name);
            if (existingBrand == null)
            {
                return new ResponseDto
                {
                    Message = "Brand not found !",
                    StatusCode = 404,
                    IsSucceeded = false,
                };
            }
            var brandDto = _mapper.Map<BrandDto>(existingBrand);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                model = brandDto
            };
        }

        public async Task<ResponseDto> UpdateBrand(int id, BrandDto brand)
        {
            if(brand == null)
            {
                return new ResponseDto
                {
                    Message = "Brand data can not be null !",
                    StatusCode = 404,
                    IsSucceeded = false,
                };
            }
            var existingBrand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);
            if (existingBrand == null)
            {
                return new ResponseDto
                {
                    Message = "Brand not found !",
                    StatusCode = 404,
                    IsSucceeded = false,
                };
            }
            _mapper.Map(brand,existingBrand);
            await _context.SaveChangesAsync();
            var brandDto = _mapper.Map<BrandDto>(existingBrand);
            return new ResponseDto
            {
                Message = "Brand updated successfully",
                StatusCode = 200,
                IsSucceeded = true,
                model = brandDto
            };
        }
    }
}
