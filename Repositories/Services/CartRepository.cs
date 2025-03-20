using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestToken.Data;
using TestToken.DTO;
using TestToken.DTO.CartDtos;
using TestToken.DTO.UserDtos;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;
using TestToken.Repositories.Interfaces;

namespace TestToken.Repositories.Services
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CartRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper) : base(context)
        {
            _usermanager = userManager;
            _context = context;
            _mapper = mapper;
        }
        public async Task<ResponseDto> AddCart(string customerId)
        {
         var existingUser= await _context.Users.AnyAsync(u=>u.Id==customerId);
            if (!existingUser)
            {
                return new ResponseDto
                {
                    Message="User not found!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            CartDto dto = new()
            {
                CustomerId = customerId,
            };
            var newCart = _mapper.Map<Cart>(dto);
            await _context.AddAsync(newCart);
            await _context.SaveChangesAsync();
            var updatedCart = _mapper.Map<CartDto>(newCart);
            return new ResponseDto
            {
                Message = "New cart added successfully!",
                IsSucceeded= true,
                StatusCode = 201
            };
        }
        public async Task<ResponseDto> ApplyDiscountCode(int id, DiscountCodeDto discountPercentage)
        {
            if(!discountPercentage.IsActive)
            {
                return new ResponseDto
                {
                    Message = "Discount not valid!!",
                    IsSucceeded = false,
                    StatusCode = 400
                };
            }
            var cart = await _context.Carts.FindAsync(id);
            if(cart == null)
            {
                return new ResponseDto
                {
                    Message = "cart not found!!",
                    IsSucceeded = false,
                    StatusCode = 400
                };
            }
            cart.TotalPrice *= (1 - discountPercentage.Percentage / 100);
            await _context.SaveChangesAsync();  
            var updatedCart = _mapper.Map<CartDto>(cart);
            return new ResponseDto
            {
                Message = "Discount applied sueccfully",
                IsSucceeded = true,
                StatusCode = 200,
                model = updatedCart
            };

            
        }
        public async Task<ResponseDto> DeleteCart(int id)
        {
            var existingCart = await _context.Carts.FirstOrDefaultAsync(c => c.Id == id);
            if (existingCart == null)
            {
                return new ResponseDto
                {
                    Message = "Cart not found!",
                    IsSucceeded =false,
                    StatusCode = 404
                };
            }
            _context.Carts.Remove(existingCart);
            await _context.SaveChangesAsync();
            return new ResponseDto
            {
                Message = "Cart deleted sucessfully",
                IsSucceeded = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> GetAllCart()
        {
            List<Cart> carts = await _context.Carts.AsNoTracking().ToListAsync();
            if(carts.Count == 0)
            {
                return new ResponseDto
                {
                    Message = "No carts found",
                    IsSucceeded = false,
                    StatusCode = 404,
                    model = new List<Cart>()
                };
            }
            var dto = _mapper.Map<List<CartDto>>(carts);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode =200,
                model = dto
            };
        }

        public async Task<ResponseDto> GetCart(int id)
        {
            var existingCart = await _context.Carts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (existingCart == null)
            {
                return new ResponseDto
                {
                    Message = "Cart not found!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            var dto = _mapper.Map<CartDto>(existingCart);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                model = dto
            };
        }

        public async Task<ResponseDto> UpdateCart(int id, CartDto cart)
        {
            var existingCart = await _context.Carts.FirstOrDefaultAsync(c => c.Id == id);
            if (existingCart == null)
            {
                return new ResponseDto
                {
                    Message = "Cart not found!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            _mapper.Map(cart, existingCart);
            await _context.SaveChangesAsync();  
            var cartDto = _mapper.Map<CartDto>(existingCart);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                model = cartDto
            };
        }
    }
}
