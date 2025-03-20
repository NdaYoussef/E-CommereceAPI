using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestToken.Data;
using TestToken.DTO;
using TestToken.DTO.OrderDto;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;
using TestToken.Repositories.Interfaces;

namespace TestToken.Repositories.Services
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderRepository(ApplicationDbContext context ,IMapper mapper,UserManager<ApplicationUser> userManager,IHttpContextAccessor httpContextAccessor) :base(context)
        {
             _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseDto> GetAllOrders()
        {
            List<Order> orders = await _context.Orders.AsNoTracking().ToListAsync();
            if(orders.Count == 0)
            {
                return new ResponseDto
                {
                    Message = "No orders found !",
                    IsSucceeded = false,
                    StatusCode = 404 ,
                    model = new List<Order>()
                };
            }
            var orderlist = _mapper.Map<List<OrderDto>>(orders);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                model = orderlist
            };
        }

        public async Task<ResponseDto> GetOrederById(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return new ResponseDto
                {
                    Message = "Order not found !",
                    IsSucceeded = false,
                    StatusCode = 404,
                };
            }
            var Order = _mapper.Map<OrderDto>(order);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                model = Order
            };
        }
        private async Task<ApplicationUser> GetCurrentUser()
        {
            var userClaim = _httpContextAccessor.HttpContext!.User;
            return await _userManager.GetUserAsync(userClaim);
        }
        public async Task<ResponseDto> CreateOrder(OrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            var currentUser = await GetCurrentUser();
            if (currentUser != null)
                order.CustomerId = currentUser.Id;

            await _context.Orders.AddAsync(order);
            var changes =  await _context.SaveChangesAsync();
            if (changes > 0)
                return new ResponseDto
                {
                    Message = "Order created successfully",
                    StatusCode = 201,
                    IsSucceeded = true,
                    model = orderDto
                };
            return new ResponseDto
            {
                IsSucceeded = false,
                StatusCode = 400,
                Message = "Failed to add this Order."
            };
        }
        public async Task<ResponseDto> UpdateOrder(int id, OrderDto order)
        {
            var existingOrder = await _context.Orders.FindAsync(id);
            if (existingOrder == null)
            {
                return new ResponseDto
                {
                    Message = "Order not found !",
                    IsSucceeded = false,
                    StatusCode = 404,
                };
            }
            _mapper.Map(order, existingOrder);
            await _context.SaveChangesAsync();
            var updatedOrder = _mapper.Map<OrderDto>(existingOrder);
             return new ResponseDto
            {
                 Message = "Order updated successfully ",
                 IsSucceeded = true,
                 StatusCode = 200,
                 model = updatedOrder
            };
            }
        public async Task<ResponseDto> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return new ResponseDto
                {
                    Message = "Order not found !",
                    IsSucceeded = false,
                    StatusCode = 404,
                };
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return new ResponseDto
            {
                Message = "Order has been deleted successfully",
                IsSucceeded = false,
                StatusCode = 200,
            };
        }
    }
}
