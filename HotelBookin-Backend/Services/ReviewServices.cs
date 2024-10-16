using AutoMapper;
using HotelBookin_Backend.Custom_Exceptions;
using HotelBookin_Backend.Data;
using HotelBookin_Backend.DTO;
using HotelBookin_Backend.IServices;
using HotelBookin_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookin_Backend.Services
{
    /// <summary>
    /// Service for managing reviews related operations.
    /// </summary>
    public class ReviewServices : IReviewService
    {
        private readonly HotelBookingDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewServices"/> class.
        /// </summary>
        /// <param name="context">Database context for accessing review data.</param>
        /// <param name="mapper">Mapper for converting between Review and ReviewDTO objects.</param>
        public ReviewServices(HotelBookingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds a new review asynchronously.
        /// </summary>
        /// <param name="reviewDto">The DTO containing review information.</param>
        /// <returns>ReviewDTO object representing the added review.</returns>
        /// <exception> ArgumentNullExceptionThrown when reviewDto is null.</exception>
        /// <exception> InvalidOperationException Thrown when mapping fails.</exception>
        public async Task<ReviewDTO> AddReviewAsync(ReviewDTO reviewDto)
        {
            try
            {
                if (reviewDto == null)
                {
                    throw new ArgumentNullException(nameof(reviewDto));
                }

                var review = _mapper.Map<Review>(reviewDto);

                if (review == null)
                {
                    throw new InvalidOperationException("Failed to map ReviewDto to Review");
                }
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();
                await _context.Entry(review).ReloadAsync(); // Reload to get the generated ID and other values

                return _mapper.Map<ReviewDTO>(review);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the review", ex);
            }
        }

        /// <summary>
        /// Deletes a review by its ID asynchronously.
        /// </summary>
        /// <param name="reviewId">The ID of the review to delete.</param>
        public async Task DeleteReviewAsync(int reviewId)
        {
            try { 
            var review = await _context.Reviews.FindAsync(reviewId);
               if (review == null) throw new ReviewNotFoundException(reviewId);
                if (review != null)
                {
                    _context.Reviews.Remove(review);
                    await _context.SaveChangesAsync();
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all reviews asynchronously.
        /// </summary>
        /// <returns>A collection of ReviewDTO objects representing all reviews.</returns>
        public async Task<IEnumerable<ReviewDTO>> GetAllReviewsAsync()
        {
            try
            {
                var reviews = await _context.Reviews.ToListAsync();
                return _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a review by its ID asynchronously.
        /// </summary>
        /// <param name="reviewId">The ID of the review to retrieve.</param>
        /// <returns>ReviewDTO object representing the review.</returns>
        public async Task<ReviewDTO> GetReviewByIdAsync(int reviewId)
        {
            try
            {
                var review = await _context.Reviews.FindAsync(reviewId);
                if (review == null) throw new ReviewNotFoundException(reviewId);
                return _mapper.Map<ReviewDTO>(review);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all reviews for a specific hotel by its ID asynchronously.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel to retrieve reviews for.</param>
        /// <returns>A collection of ReviewDTO objects representing the hotel's reviews.</returns>
        public async Task<IEnumerable<ReviewDTO>> GetReviewsByHotelAsync(int hotelId)
        {
            try
            {
                var reviews = await _context.Reviews.Where(r => r.HotelId == hotelId).ToListAsync();
                var review = await _context.Reviews.FirstOrDefaultAsync(r=>r.HotelId==hotelId);
                if (review == null) throw new HotelNotFoundException(hotelId);
                return _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all reviews made by a specific user by their ID asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve reviews for.</param>
        /// <returns>A collection of ReviewDTO objects representing the user's reviews.</returns>
        public async Task<IEnumerable<ReviewDTO>> GetReviewsByUserAsync(int userId)
        {
            try
            {
                var reviews = await _context.Reviews.Where(r => r.UserId == userId).ToListAsync();
                var review = await _context.Reviews.FirstOrDefaultAsync(r => r.UserId == userId);
                if (review == null) throw new UserNotFoundException(userId);
                return _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing review by its ID asynchronously.
        /// </summary>
        /// <param name="reviewId">The ID of the review to update.</param>
        /// <param name="reviewDto">The DTO containing updated review information.</param>
        /// <returns>A ReviewDTO object representing the updated review.</returns>
        public async Task<ReviewDTO> UpdateReviewAsync(int reviewId, ReviewDTO reviewDto)
        {
            try
            {
                var review = await _context.Reviews.FindAsync(reviewId);
                if (review != null)
                {
                    _mapper.Map(reviewDto, review);
                    await _context.SaveChangesAsync();
                    return _mapper.Map<ReviewDTO>(review);
                }
                return null; // Return null if the review was not found
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
