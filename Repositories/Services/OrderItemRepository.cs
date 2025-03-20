using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestToken.Data;
using TestToken.DTO;
using TestToken.DTO.OrderDto;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;
using TestToken.Repositories.Interfaces;

namespace TestToken.Repositories.Services
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public OrderItemRepository(ApplicationDbContext context,IMapper mapper):base(context)
        {
             _context = context;    
            _mapper = mapper;

        }
      

        public async Task<ResponseDto> GetAllItems()
        {
            List<OrderItem> orderItems = await _context.OrderItems.AsNoTracking().Include(p=>p.Product).ToListAsync();
            if (!orderItems.Any()&&orderItems.Count == 0)
            {
                return new ResponseDto
                {
                    Message = "No items found!",
                    IsSucceeded = false,
                    StatusCode = 404,
                    model = new List<OrderItem>()
                };
            }
            var ItemsDto = _mapper.Map<List<OrderItemDto>>(orderItems);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                model = ItemsDto
            };
        }

        public async Task<ResponseDto> GetItemByID(int id)
        {
            var orderItem = await _context.OrderItems.Where(i => i.Id == id).Include(p => p.Product).FirstOrDefaultAsync();
            if (orderItem == null)
            {
                return new ResponseDto
                {
                    Message = "No item found!",
                    IsSucceeded = false,
                    StatusCode = 404,
                };
            }
            var ItemDto = _mapper.Map<OrderItemDto>(orderItem);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                model = ItemDto
            };
        }
        public async Task<ResponseDto> AddItem(OrderItemDto item)
        {
            var productExists = await _context.Products.AnyAsync(o => o.Id == item.ProductId);
            if (!productExists)
            {
                return new ResponseDto
                {
                    Message = "Product is not found!!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            var orderExists = await _context.Orders.AnyAsync(o => o.Id == item.OrderId);
            if (!orderExists)
            {
                return new ResponseDto
                {
                    Message = "Order not found!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            var orderItem = _mapper.Map<OrderItem>(item);
            await _context.AddAsync(orderItem);
            await _context.SaveChangesAsync();
            var itemDto = _mapper.Map<OrderItemDto>(orderItem);
            return new ResponseDto
            {
                Message = "Item added successfully",
                IsSucceeded = true,
                StatusCode = 201,
                model = itemDto
            };
        }
        public async Task<ResponseDto> UpdateItem(int id, OrderItemDto item)
        {
            var existingItem = await _context.OrderItems
                .Include(o=>o.Order)
                .Include(o=>o.Product)
                .FirstOrDefaultAsync(o=>o.Id==id);
            if (existingItem == null)
            {
                return new ResponseDto
                {
                    Message = "No item found!",
                    IsSucceeded = false,
                    StatusCode = 404,
                };
            }
            var productExists = await _context.Products.AnyAsync(p => p.Id == item.ProductId);
            var orderExists = await _context.Orders.AnyAsync(o => o.Id == item.OrderId);

            if (!productExists)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    StatusCode = 400,
                    Message = "The Product does not exist."
                };
            }

            if (!orderExists)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    StatusCode = 400,
                    Message = "The Order does not exist."
                };
            }
            existingItem.ProductId = item.ProductId;
            existingItem.OrderId = item.OrderId;
            existingItem.Quantity = item.Quantity;
            var itemDto = _mapper.Map<OrderItemDto>(existingItem);

            await _context.SaveChangesAsync();
            return new ResponseDto
            {
                Message = "Item updated successfully",
                IsSucceeded = true,
                StatusCode = 200,
                model = itemDto
            };
        }
        public async Task<ResponseDto> DeleteItem(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return new ResponseDto
                {
                    Message = "No item found!",
                    IsSucceeded = false,
                    StatusCode = 404,
                };
            }
            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
            var itemDto = _mapper.Map<OrderItemDto>(orderItem);
            return new ResponseDto
            {
                Message = "Item deleted successfully",
                IsSucceeded = true,
                StatusCode = 200,
                model = itemDto
            };
        }
    }
}
