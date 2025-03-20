using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestToken.Data;
using TestToken.DTO;
using TestToken.DTO.CartDtos;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;
using TestToken.Repositories.Interfaces;

namespace TestToken.Repositories.Services
{
    public class CartItemRepository : GenericRepository<CartItem>, ICartItemRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CartItemRepository(ApplicationDbContext context,IMapper mapper):base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDto> GetAllItems()
        {
            List<CartItem> cartItems = await _context.CartItems.AsNoTracking().Include(p=>p.Product).ToListAsync();
            if(!cartItems.Any())
            {
                return new ResponseDto
                {
                    Message = "Items not found!!",
                    IsSucceeded = false,
                    StatusCode = 404,
                    model = new List<CartItem>()
                };
            }
            var CartList =_mapper.Map<List<CartItemDto>>(cartItems);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                model = CartList
            };
        }
        public async Task<ResponseDto> GetItemById(int id)
        {
            var existingItem = await _context.CartItems.Where(c=>c.Id==id).Include(p=>p.Product).FirstOrDefaultAsync();
            if(existingItem == null)
            {
                return new ResponseDto
                {
                    Message="Item not found!!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            var cartItem = _mapper.Map<CartItemDto>(existingItem);
            return new ResponseDto
            {
                IsSucceeded= true,
                StatusCode = 200,
                model = cartItem
            };
        }
        public async Task<ResponseDto> AddItem(CartItemDto item)
        {
            var cartExists = await _context.Carts.AnyAsync(c => c.Id == item.CartId);
            if (!cartExists)
            {
                return new ResponseDto
                {
                    Message = "Cart not found!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            var productExists = await _context.Products.AnyAsync(p => p.Id == item.ProductId);
            if (!productExists)
            {
                return new ResponseDto
                {
                    Message = "product you try to add doesn't exist!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            var addedItem = _mapper.Map<CartItem>(item);
            _context.CartItems.Add(addedItem);
            await _context.SaveChangesAsync();
            var dto = _mapper.Map<CartItemDto>(addedItem);
            return new ResponseDto
            {
                Message = "Item added to cart successfully",
                IsSucceeded = true,
                StatusCode = 201,
                model = dto
            };
        }

        public async Task<ResponseDto> UpdateItem(int id, CartItem cartItem)
        {
            var existingItem = await _context.CartItems.FindAsync(id);
            if(existingItem == null)
            {
                return new ResponseDto
                {
                    Message="item not found!",
                    IsSucceeded = false,
                    StatusCode =404
                };
            }
            var productExists = await _context.Products.AnyAsync(p => p.Id == cartItem.ProductId);
            if (!productExists)
            {
                return new ResponseDto
                {
                    Message = "Product not found!",
                    IsSucceeded = false,
                    StatusCode = 400
                };
            }
            existingItem.Quantity=cartItem.Quantity;
            existingItem.ProductId=cartItem.ProductId;
           // _context.CartItems.Update(existingItem);
            await _context.SaveChangesAsync();
            var updatedItem = _mapper.Map<CartItemDto>(existingItem);
            return new ResponseDto
            { 
                    Message = "Item updated successfully",
                    IsSucceeded = true,
                    StatusCode = 200,
                    model = updatedItem
            };
        }
        public async Task<ResponseDto> DeleteItem(int id)
        {
            var existingItem = await _context.CartItems.FindAsync(id);
            if (existingItem == null)
            {
                return new ResponseDto
                {
                    Message = "Item not found!!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            _context.CartItems.Remove(existingItem);
            await _context.SaveChangesAsync();
            return new ResponseDto
            {
                Message = "Item deleted sucessfully",
                IsSucceeded = true,
                StatusCode = 200
            };
        }
    }
}
