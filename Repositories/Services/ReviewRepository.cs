using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestToken.Data;
using TestToken.DTO;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;
using TestToken.Repositories.Interfaces;

namespace TestToken.Repositories.Services
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ReviewRepository(ApplicationDbContext context ,IMapper mapper) : base(context) 
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDto> GetAllReviewsAsync()
        {
            var reviews = await _context.Reviews
                .Include(p => p.Product)
                .ToListAsync();

            if (reviews.Count is 0)
            {
                return new ResponseDto
                {
                    Message = "No reviews found!",
                    IsSucceeded = false,
                    StatusCode = 404,
                    model = null
                };
            }

            var reviewsDto = _mapper.Map<List<ReviewDto>>(reviews);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                model = reviewsDto
            };
        }
        public async Task<ResponseDto> GetReviewById(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
                return new ResponseDto
                {
                    Message = "No reviwe found!",
                    IsSucceeded = false,
                    StatusCode = 404,
                };
            var reviewDto = _mapper.Map<ReviewDto>(review);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                model = reviewDto,
            };
        }
        public async Task<ResponseDto> AddReviewAsync(ReviewDto reviewDto, string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                return new ResponseDto
                {
                    Message = "Invalid Customer Id!",
                    IsSucceeded = false,
                    StatusCode = 400
                };
            }
            var userExists = await _context.Users.AnyAsync(u => u.Id == customerId);
            if (!userExists)
            {
                return new ResponseDto
                {
                    Message = "Customer does not exist!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            var review = _mapper.Map<Review>(reviewDto);
            review.CustomerId = customerId;
            var productExists = await _context.Products.AnyAsync(p => p.Id == reviewDto.ProductId);
            if (!productExists)
            {
                return new ResponseDto
                {
                    Message = "Product you are trying to review is not available!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return new ResponseDto
            {
                Message = "Review has been added successfully",
                IsSucceeded = true,
                StatusCode = 201,
                model = review
            };
        }

        public async Task<ResponseDto> UpdateReviewAsync(int id, ReviewDto review,string customerId)
        {
            var updatedReview = await _context.Reviews.FindAsync(id);
            if (updatedReview == null)
                return new ResponseDto
                {
                    Message = "No reviwe found!",
                    IsSucceeded = false,
                    StatusCode = 404,
                };
            if (updatedReview.CustomerId != customerId)
            {
                return new ResponseDto
                {
                    Message = "Unauthorized! You can only update your own reviews.",
                    IsSucceeded = false,
                    StatusCode = 403,
                };
            }
            var productExist = await _context.Products.AnyAsync(p => p.Id == review.ProductId);
            if (!productExist)
            {
                return new ResponseDto
                {
                    Message = "The product you are trying to review does not exist.",
                    IsSucceeded = false,
                    StatusCode = 400,
                };
            }
            updatedReview.Rate = review.Rate;
            updatedReview.Comment = review.Comment;
            updatedReview.ProductId = review.ProductId;
          //  _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
            var reviewDto = _mapper.Map<ReviewDto>(updatedReview);
            return new ResponseDto
            {
                Message = "Your Review updated successfully",
                IsSucceeded = true,
                StatusCode = 200,
                model = reviewDto
            };

        }
        public async Task<ResponseDto> DeleteReviewAsync(int id, string customerId)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == id && r.CustomerId == customerId);
            if (review == null)
                return new ResponseDto
                {
                    Message = "No review found!",
                    IsSucceeded = false,
                    StatusCode = 404,
                };
            //if(review.CustomerId != customerId)
            //{
            //    return new ResponseDto
            //    {
            //        Message = "You are not authorized to delete this review.",
            //        IsSucceeded = false,
            //        StatusCode = 403,
            //    };
            //}
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return new ResponseDto
            {
                Message = "Review has been deleted successfully",
                IsSucceeded = true,
                StatusCode = 200,
            };
        }
    }
}
