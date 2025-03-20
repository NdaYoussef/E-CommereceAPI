using TestToken.DTO;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;

namespace TestToken.Repositories.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<ResponseDto> GetAllReviewsAsync();
        Task<ResponseDto> GetReviewById(int id);
        Task<ResponseDto> AddReviewAsync(ReviewDto reviewDto, string customerId);
        Task<ResponseDto> UpdateReviewAsync(int id , ReviewDto review, string customerId);
        Task<ResponseDto> DeleteReviewAsync(int id, string customerId);
    }
}
