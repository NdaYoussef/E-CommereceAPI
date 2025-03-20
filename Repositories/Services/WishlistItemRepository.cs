using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MimeKit.Tnef;
using TestToken.Data;
using TestToken.DTO;
using TestToken.DTO.WishlistDto;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;
using TestToken.Repositories.Interfaces;

namespace TestToken.Repositories.Services
{
    public class WishlistItemRepository : GenericRepository<WishListItem>, IWishListItemRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public WishlistItemRepository(ApplicationDbContext context,IMapper mapper) :base(context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ResponseDto> GetAllWishlistItems()
        {
            var items = await _context.WishlistItems.AsNoTracking().ToListAsync();
            if(items.Count == 0)
            {
                return new ResponseDto
                {
                    Message = "No items found!",
                    IsSucceeded = false,
                    StatusCode = 404,
                    model = new List<WishListItem>()

                };
            }
            var listedItems = _mapper.Map<List<WishlistItemsDto>>(items);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                model = listedItems
            };
        }

        public async Task<ResponseDto> GetItemInWishList(int listId)
        {
           var wishList = await _context.Wishlists.FindAsync(listId);
            if (wishList == null)
                return new ResponseDto
                {
                    Message = "This list is unavailable",
                    IsSucceeded = false,
                    StatusCode = 404
                };

            var items = await _context.WishlistItems.Where(w => w.Id == listId)
                                                    .Include(w => w.Wishlist)
                                                    .Include(w => w.Product)
                                                    .ToListAsync();
            if (items.Count == 0)
                return new ResponseDto
                {
                    Message = "No items found!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            var itemsDto = _mapper.Map<List<WishlistItemsDto>>(items);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                model = itemsDto
            };
        }

        public async Task<ResponseDto> GetWishlistItemById(int id)
        {
            var item = await _context.WishlistItems
                                                    .Include(w => w.Wishlist)
                                                    .Include(w => w.Product)
                                                    .FirstOrDefaultAsync(w=>w.Id==id);
            if (item is null)
                return new ResponseDto
                {
                    Message = "No Item found!",
                    IsSucceeded = false,
                    StatusCode = 404
                };

            var itemDto = _mapper.Map<WishlistItemsDto>(item);
            return new ResponseDto
            {
                IsSucceeded= true,
                StatusCode = 200,
                model = itemDto
            };
        }

        public async Task<ResponseDto> AddItemtoWishlist(WishlistItemsDto item)
        {
           var wishlist = await _context.Wishlists.FindAsync(item.WishlistId);
           var product = await _context.Products.FindAsync(item.ProductId);
            if (wishlist == null)
                return new ResponseDto
                {
                    Message = "Wish list you try to add item to is not found.!",
                    IsSucceeded= false,
                    StatusCode = 404
                };
            if (product == null)
                return new ResponseDto
                {
                    Message = "Product you try to add is not found.!",
                    IsSucceeded= false,
                    StatusCode = 404
                };
            var existingItem = await _context.WishlistItems
                                            .FirstOrDefaultAsync(w => w.WishlistId == item.WishlistId && w.ProductId == item.ProductId);

            if (existingItem != null)
            {
                return new ResponseDto
                {
                    Message = "Item is already in the wishlist!",
                    IsSucceeded = false,
                    StatusCode = 400
                };
            }
            var wishlistItem = new WishListItem
            { 
             WishlistId = item.WishlistId,
             ProductId = item.ProductId,
            };

            await _context.WishlistItems.AddAsync(wishlistItem);
            await _context.SaveChangesAsync();
            return new ResponseDto
            {
                Message = "Item added successfully",
                StatusCode = 200,
                IsSucceeded = true,
                model = item
            };
        }
        public async Task<ResponseDto> UpdateWishlistItem(int id, WishlistItemsDto item)
        {
            var existingItem = await _context.WishlistItems.FindAsync(id);
            if (existingItem == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    Message = "Wishlist item not found."
                };
            }
            var existingList = await _context.Wishlists.AnyAsync(w => w.Id == item.WishlistId);
            var existingProduct = await _context.Products.AnyAsync(p=>p.Id == item.ProductId);
            if (!existingList)
            {
                return new ResponseDto
                {
                    StatusCode = 400,
                    IsSucceeded = false,
                    Message = "Wishlist not found."
                };
            }
            if (!existingProduct)
            {
                return new ResponseDto
                {
                    StatusCode = 400,
                    IsSucceeded = false,
                    Message = "Product not found."
                };
            }
            existingItem.WishlistId = item.WishlistId;
            existingItem.ProductId = item.ProductId;
            await _context.SaveChangesAsync();
            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Message = "Wishlist item updated successfully."
            };

        }
        public async Task<ResponseDto> DeleteWishlistItem(int id)
        {
           var Item = await _context.WishlistItems.FindAsync(id);
            if (Item == null)
                return new ResponseDto
                {
                    Message = "No item found!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            _context.WishlistItems.Remove(Item);
            await _context.SaveChangesAsync();
            var itemDto = _mapper.Map<WishlistItemsDto>(Item);
            return new ResponseDto
            {
                Message ="Item deleted sueccfully!",
                IsSucceeded= true,
                StatusCode = 200,
                model = itemDto    
            };

        }

      
     
    }
}
