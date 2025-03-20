using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestToken.Data;
using TestToken.DTO;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;
using TestToken.Repositories.Interfaces;

namespace TestToken.Repositories.Services
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CategoryRepository(ApplicationDbContext context ,IMapper mapper):base(context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ResponseDto> AddCategory(CategoryDto category)
        {
            var newCategory = _mapper.Map<Category>(category);
            await _context.AddAsync(newCategory);
            await _context.SaveChangesAsync();
            var addedCategory = _mapper.Map<CategoryDto>(newCategory);
            return new ResponseDto
            {
                Message = "New category has been addded",
                IsSucceeded = true,
                StatusCode = 200,
                model = addedCategory
            };
        }

        public async Task<ResponseDto> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return new ResponseDto
                {
                    Message = "Category not found!",
                    IsSucceeded = false,
                    StatusCode = 404,
                };
            }
             _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return new ResponseDto
            {
                Message = "Category deleted successfully",
                IsSucceeded = true,
                StatusCode = 200
            };
                    
        }

        public async Task<ResponseDto> GetAllCategories()
        {
            List<Category> categories = await _context.Categories.AsNoTracking().ToListAsync();
            if(!categories.Any())
            {
                return new ResponseDto
                {
                    Message = "No categories found.",
                    IsSucceeded = false,
                    StatusCode = 404 , 
                    model = new List<CategoryDto>()
                };
            }
            var existingCategories = _mapper.Map<List<CategoryDto>>(categories);
            return new ResponseDto
            {
                IsSucceeded = true , 
                StatusCode = 200 ,
                model = existingCategories
            };
        }

        public async Task<ResponseDto> GetCategoryById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return new ResponseDto
                {
                    Message = "Category not found!",
                    IsSucceeded = false,
                    StatusCode = 404,
                };
            }
            var existingCategory = _mapper.Map<CategoryDto>(category);
            return new ResponseDto
            {
                IsSucceeded = true ,
                StatusCode= 200 ,
                model = existingCategory
            };
        }

        public async Task<ResponseDto> GetCategoryByName(string name)
        {
            var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c=>c.Name == name);
            if (category == null)
            {
                return new ResponseDto
                {
                    Message = "Category not found!",
                    IsSucceeded = false,
                    StatusCode = 404,
                };
            }
            var existingCategory = _mapper.Map<CategoryDto>(category);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                model = existingCategory
            };
        }

        public async Task<ResponseDto> UpdateCategory(int id, CategoryDto category)
        {
            var existingCategory = await _context.Categories.FindAsync(id);
            if(existingCategory == null)
            {
                return new ResponseDto
                {
                    Message = "Category not found!",
                    IsSucceeded = false,
                    StatusCode = 404,
                };
            }
            existingCategory.Name = category.Name;
            await _context.SaveChangesAsync();
            var updatedCatgeory = _mapper.Map<CategoryDto>(category);
            return new ResponseDto
            {
                Message = "Caategory updated successfully",
                IsSucceeded = true,
                StatusCode = 200,
                model = updatedCatgeory
            };
        }
    }
}
