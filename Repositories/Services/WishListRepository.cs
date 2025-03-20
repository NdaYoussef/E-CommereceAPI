using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestToken.Data;
using TestToken.DTO;
using TestToken.DTO.WishlistDto;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;
using TestToken.Repositories.Interfaces;

namespace TestToken.Repositories.Services
{
    public class WishListRepository : GenericRepository<WishList>, IWishListRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        public WishListRepository(ApplicationDbContext context , IMapper mapper,IEmailService emailService) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
        }
        public async Task<ResponseDto> GetAllWishlistAsync()
        {
            var wishList = await _context.Wishlists.AsNoTracking().ToListAsync();
            if (wishList.Count == 0)
            {
                return new ResponseDto
                {
                    Message = "No wishlists found!",
                    IsSucceeded = false,
                    StatusCode = 404,
                    model = new List<WishList>(wishList)
                };
            }
            var wishlistDto = _mapper.Map<List<WishlistDto>>(wishList);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                model = wishlistDto,
            };
        }

        public async Task<ResponseDto> GetWishlistByIdAsync(int id)
        {
            var wishlist = await _context.Wishlists.FindAsync(id);
            if (wishlist == null)
            {
                return new ResponseDto
                {
                    Message = "No whislist found!",
                    IsSucceeded = false,
                    StatusCode = 404 , 
                };
            }
            var wishlistDto = _mapper.Map<WishlistDto>(wishlist);
            return new ResponseDto
            {
                Message = "wishlist retrived successfully",
                IsSucceeded = true,
                StatusCode = 200,
                model = wishlistDto,
            };
        }

        public async Task<ResponseDto> AddWishlistAsync(WishlistDto wishlistDto, string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                return new ResponseDto
                {
                    Message = "Customer ID is required",
                    IsSucceeded = false,
                    StatusCode = 400
                };
            }
            var user = await _context.Users.FindAsync(customerId);
            if (user == null)
            {
                return new ResponseDto
                {
                    Message = "no user found!",
                    IsSucceeded = false,
                    StatusCode = 404,
                };
            }
            wishlistDto.CustomerId = customerId;
            var wishlist = new WishList
            {
                Name = wishlistDto.Name,
                CustomerId = customerId,
                WishlistItems = new List<WishListItem>()
            };
           
            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();
            return new ResponseDto
            {
                Message = "wishlists created successfully",
                IsSucceeded = true,
                StatusCode = 201,
                model = wishlistDto,
            };
        }      
        public async Task<ResponseDto> UpdateWishlistAsync(int id, WishlistDto wishlistDto)
        {
            var wishlist = await _context.Wishlists.FindAsync(id);
            if (wishlist == null)
            {
                return new ResponseDto
                {
                    Message = "No whislist found!",
                    IsSucceeded = false,
                    StatusCode = 404,
                };
            }
            _mapper.Map(wishlistDto, wishlist); //source => destination
             
            _context.Wishlists.Update(wishlist);
            await _context.SaveChangesAsync();
            return new ResponseDto
            {
                Message = "Wishlist updated successfully",
                IsSucceeded = true,
                StatusCode = 200,
                model = wishlistDto,
            };
        }
        public async Task<ResponseDto> DeleteWishlistAsync(int id)
        {
            var wishlist = await _context.Wishlists.FindAsync(id);
            if (wishlist == null)
            {
                return new ResponseDto
                {
                    Message = "No whislist found!",
                    IsSucceeded = false,
                    StatusCode = 404,
                };
            }
       
             _context.Wishlists.Remove(wishlist);
            await _context.SaveChangesAsync();
            return new ResponseDto
            {
                Message = "wishlist deleted successfully",
                IsSucceeded = true,
                StatusCode = 200
            };
        }

        public async Task<bool> ShareWishlistAsync(int id, string email)
        {

            if (string.IsNullOrEmpty(email)) return false;
            var wishlist = await _context.Wishlists.FindAsync(id);
            if (wishlist == null) return false;
            var wishlistLink = $"https://localhost:7154/api/Wishlist/{id}";
            var subject = "Shared Wishlist";
            var message = $"A wishlist has been shared with you. Check it out: {wishlistLink}";
            if (_emailService != null)
            {
                await _emailService.sendEmailAsync(email, subject, message);
                return true;
            }

            return true;
        }
    }
}
