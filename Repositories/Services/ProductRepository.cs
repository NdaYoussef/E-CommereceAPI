using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestToken.Data;
using TestToken.DTO;
using TestToken.DTO.ProductDto;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;
using TestToken.Repositories.Interfaces;

namespace TestToken.Repositories.Services
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper; 
        public ProductRepository(ApplicationDbContext context , IMapper mapper):base(context)
        {
             _context = context;
            _mapper = mapper;
        }
        public async Task<ResponseDto> AddProduct(ProductDto productDto)
        {
            var existingBrand = await _context.Brands.FindAsync(productDto.BrandId);
            var existingCategory = await _context.Categories.FindAsync(productDto.categoryId);
            if (existingBrand is null || existingCategory is null)
            {
                return new ResponseDto
                {
                    Message = "Invalid request!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            var existingProduct = await _context.Products
                   .AsNoTracking()
                   .FirstOrDefaultAsync(p => p.Name == productDto.Name);

            if (existingProduct != null)
            {
                return new ResponseDto
                {
                    Message = "A product with the same name already exists!",
                    IsSucceeded = false,
                    StatusCode = 400
                };
            }
            var product =  _mapper.Map<Product>(productDto);
             _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return new ResponseDto
            {
                Message = "Product has been added successfully",
                IsSucceeded = true,
                StatusCode = 201,
                model = _mapper.Map<ProductDto>(product)
            };
            }

        public async Task<ResponseDto> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return new ResponseDto
                {
                    Message = "Product not found!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            var existingProduct = _mapper.Map<ProductDto>(product);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                model = existingProduct
            };
        }

        public async Task<ResponseDto> GetAllProducts()
        {
           List<Product> products = await _context.Products
                .Include(c=>c.Category)
                .Include(b=>b.Brand)
                .AsNoTracking().ToListAsync();
            if (!products.Any()&&products.Count==0)
            {
                return new ResponseDto
                {
                    Message = "No products found!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            var exidtingProduct = _mapper.Map<List<ProductDto>>(products);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                model = exidtingProduct
            };
        }
        public async Task<ResponseDto> GetProductById(int id)
        {
           var existingProduct = await _context.Products
                .Include(c=>c.Category)
                .Include(b=>b.Brand)
                .AsNoTracking().FirstOrDefaultAsync();
            if(existingProduct is null)
            {
                return new ResponseDto
                {
                    Message = "Product not found!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            var product = _mapper.Map<ProductDto>(existingProduct);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                model = product
            };
        }
        public async Task<ResponseDto> GetProductsByCategory(string categoryName)
        {
            List<Product> products = await _context.Products
                                        .Include(c=>c.Category)
                                        .Include(b=>b.Brand)
                                        .AsNoTracking().ToListAsync();
            if (!products.Any())
            {
                return new ResponseDto
                {
                    Message = "No products found!",
                    IsSucceeded = false,
                    StatusCode  = 404
                };
            }
            var productDto = _mapper.Map<List<ProductDto>>(products);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                model = productDto
            };
        }



        public async Task<ResponseDto> GetProductByName(string name)
        {
            var existingProduct = await _context.Products
                .Include(c => c.Category)
                .Include(b => b.Brand)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name == name);
            if (existingProduct is null)
            {
                return new ResponseDto
                {
                    Message = "Product not found!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            var product = _mapper.Map<ProductDto>(existingProduct);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                model = product
            };
        }

        public async Task<ResponseDto> GetProductsByBrandName(string name)
        {
            List<Product> products = await _context.Products
                                       .Include(c => c.Category)
                                       .Include(b => b.Brand)
                                       .AsNoTracking().ToListAsync();
            if (!products.Any())
            {
                return new ResponseDto
                {
                    Message = "No products found!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            var productDto = _mapper.Map<List<ProductDto>>(products);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                model = productDto
            };
        }

        public async Task<ResponseDto> UpdateProduct(int id, ProductDto productDto)
        {
            var existingBrand = await _context.Brands.FindAsync(productDto.BrandId);
            var existingCategory = await _context.Categories.FindAsync(productDto.categoryId);
            if (existingBrand is null || existingCategory is null)
            {
                return new ResponseDto
                {
                    Message = "Invalid request!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return new ResponseDto
                {
                    Message = "product not found!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            _mapper.Map(productDto, product);
            product.Brand = existingBrand;
            product.Category = existingCategory;
            _context.Entry(product).CurrentValues.SetValues(productDto);
            await _context.SaveChangesAsync();
            return new ResponseDto
            {
                Message = "Product updated successfully",
                IsSucceeded = true,
                StatusCode = 200,
                model = productDto
            };
            }
    }
}
