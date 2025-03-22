using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Runtime;
using TestToken.Data;
using TestToken.DTO.PaymentDto;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly EmailSettings _emailSettings;
        private readonly IEmailService _emailService;
        IOptions<StripeSettings> _stripeSettings;
        private readonly EmailTemplateService _emailTemplateService;



        public UnitOfWork
            (ApplicationDbContext context,UserManager<ApplicationUser> userManager,
            ITokenService tokenService, IMapper mapper,
            RoleManager<IdentityRole> roleManager,IEmailService emailService,
            IHttpContextAccessor httpContextAccessor,IOptions<EmailSettings> options,
            IOptions<StripeSettings> stripeSettings,
            EmailTemplateService emailTemplateService

            )
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _emailSettings = options.Value;
            _emailService = emailService;
            _stripeSettings = stripeSettings;
            Customers = new AccountRepository(_context, _userManager, _tokenService, _mapper, _emailService,_emailTemplateService,  _roleManager);
            Brands = new BrandRepository(_context, _userManager, _mapper);
            Carts = new CartRepository(_context, _userManager, _mapper);
            CartItems = new CartItemRepository(_context, _mapper);
            Categories = new CategoryRepository(_context, _mapper);
            Orders = new OrderRepository(_context, _mapper, _userManager, _httpContextAccessor);
            OrderItems = new OrderItemRepository(_context, _mapper);
            Products = new ProductRepository(_context, _mapper);
            //Emails = new EmailService(_emailSettings,_userManager,_context);
            Reviews = new ReviewRepository(_context, _mapper);  
            WishLists = new WishListRepository(_context, _mapper,_emailService);
            WishListItems = new WishlistItemRepository(_context, _mapper);
            Payments = new PaymentRepository(_context, _mapper,_stripeSettings);
        }
        public IAccountRepository Customers { get; private set; }
        public IBrandRepository Brands { get; private set; }

        public ICartItemRepository CartItems { get; private set; }

        public ICartRepository Carts { get; private set; }

        public ICategoryRepository Categories { get; private set; }

        public IProductRepository Products { get; private set; }

        public IPaymentRepository Payments { get; private set; }
        public IReviewRepository Reviews { get; private set; }

        public IWishListItemRepository WishListItems { get; private set; }

        public IWishListRepository WishLists { get; private set; }

        public IOrderItemRepository OrderItems { get; private set; }

        public IOrderRepository Orders { get; private set; }

        public ITokenService TokenService { get; private set; }
        public IEmailService Emails { get; private set; }



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
