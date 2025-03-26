using Microsoft.EntityFrameworkCore;
using Stripe.Tax;
using TestToken.Data;
using TestToken.DTO;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;
using TestToken.Repositories.Interfaces;

namespace TestToken.Repositories.Services
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) :base(context) 
        {
             _context = context;
        }
        public async Task<ResponseDto> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            if (users.Count == 0)
                return new ResponseDto
                {
                    Message = "No users found!",
                    IsSucceeded = false 
                };
            var result = users.Select(u => new
            {
                UserId = u.Id,
                UserName = $"{u.FirstName} {u.LastName}",
                PhoneNumber = u.PhoneNumber,
                RegistrationDate = u.RegistrationDate
            });
            return new ResponseDto
            {
                model = result,
                IsSucceeded = true
            };
        }
    }
}
