using HotelBookin_Backend.DTO;

namespace HotelBookin_Backend.IServices
{
    public interface IReviewService
    {
        Task<ReviewDTO> AddReviewAsync(ReviewDTO reviewDto);
        Task<ReviewDTO> UpdateReviewAsync(int reviewId, ReviewDTO reviewDto);
        Task DeleteReviewAsync(int reviewId);
        Task<IEnumerable<ReviewDTO>> GetAllReviewsAsync();
        Task<ReviewDTO> GetReviewByIdAsync(int reviewId);
        Task<IEnumerable<ReviewDTO>> GetReviewsByHotelAsync(int hotelId);
        Task<IEnumerable<ReviewDTO>> GetReviewsByUserAsync(int userId);
    }
}
