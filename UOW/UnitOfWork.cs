using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Runtime;
using TestToken.Data;
using TestToken.Models;
using TestToken.Repositories.Interfaces;
using TestToken.Repositories.Services;

namespace TestToken.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        public UnitOfWork(ApplicationDbContext context,UserManager<ApplicationUser> userManager,ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _tokenService = tokenService;


            Customers = new AccountRepository(_context,_userManager, _tokenService, _mapper);

        }
        public IAccountRepository Customers { get; private set; }
        public IBrandRepository Brands { get; private set; }

        public ICartItemRepository CartItems { get; private set; }

        public ICartRepository Carts { get; private set; }

        public ICategoryRepository Categories { get; private set; }

        public IProductRepository Products { get; private set; }

        public IPaymentRepository Payments { get; private set; }

        public IWishListItemRepository WishListItems { get; private set; }

        public IWishListRepository WishLists { get; private set; }

        public IOrderItemRepository OrderListItems { get; private set; }

        public IOrderRepository Orders { get; private set; }

        public ITokenService TokenService { get; private set; }
        
        
        public async Task<int> SaveCompleted()
        {
         return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
           _context.Dispose();
        }
    }
}
