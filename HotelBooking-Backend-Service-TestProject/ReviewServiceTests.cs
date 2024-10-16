using AutoMapper;
using HotelBookin_Backend.Data;
using HotelBookin_Backend.DTO;
using HotelBookin_Backend.Models;
using HotelBookin_Backend.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking_Backend_Service_TestProject
{
    [TestFixture]
    public class ReviewServicesTests
    {
        private ReviewServices _reviewService;
        private HotelBookingDbContext _context;
        private Mock<IMapper> _mockMapper;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<HotelBookingDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new HotelBookingDbContext(options);
            _mockMapper = new Mock<IMapper>();
            _reviewService = new ReviewServices(_context, _mockMapper.Object);
        }

        [Test]
        public async Task AddReviewAsync_ValidReview_ReturnsReviewDTO()
        {
            // Arrange
            var reviewDto = new ReviewDTO {  Comment = "Great stay!", HotelId = 1, UserId = 1 };
            var review = new Review { ReviewId = 1, Comment = reviewDto.Comment, HotelId = reviewDto.HotelId, UserId = reviewDto.UserId };

            _mockMapper.Setup(m => m.Map<Review>(reviewDto)).Returns(review);
            _mockMapper.Setup(m => m.Map<ReviewDTO>(review)).Returns(reviewDto);

            // Act
            var result = await _reviewService.AddReviewAsync(reviewDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Great stay!", result.Comment);
        }

        [Test]
        public void AddReviewAsync_NullReview_ThrowsArgumentNullException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _reviewService.AddReviewAsync(null));
            Assert.AreEqual("Value cannot be null. (Parameter 'reviewDto')", ex.Message);
        }

        [Test]
        public async Task DeleteReviewAsync_ValidId_DeletesReview()
        {
            // Arrange
            var review = new Review { ReviewId = 2, Comment = "Great stay!", HotelId = 1, UserId = 1 };
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            // Act
            await _reviewService.DeleteReviewAsync(1);

            // Assert
            var deletedReview = await _context.Reviews.FindAsync(1);
            Assert.IsNull(deletedReview);
        }

        [Test]
        public async Task GetAllReviewsAsync_ReturnsAllReviews()
        {
            // Arrange
            var review1 = new Review { ReviewId = 3, Comment = "Great stay!", HotelId = 1, UserId = 1 };
            var review2 = new Review { ReviewId = 4, Comment = "Okay experience.", HotelId = 1, UserId = 2 };
            _context.Reviews.AddRange(review1, review2);
            await _context.SaveChangesAsync();

            var reviewsDto = new List<ReviewDTO>
            {
                new ReviewDTO { Comment = "Great stay!" },
                new ReviewDTO {  Comment = "Okay experience." }
            };
            _mockMapper.Setup(m => m.Map<IEnumerable<ReviewDTO>>(It.IsAny<IEnumerable<Review>>())).Returns(reviewsDto);

            // Act
            var result = await _reviewService.GetAllReviewsAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetReviewByIdAsync_ValidId_ReturnsReviewDTO()
        {
            // Arrange
            var review = new Review { ReviewId = 5, Comment = "Great stay!", HotelId = 1, UserId = 1 };
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            var reviewDto = new ReviewDTO { Comment = "Great stay!" };
            _mockMapper.Setup(m => m.Map<ReviewDTO>(review)).Returns(reviewDto);

            // Act
            var result = await _reviewService.GetReviewByIdAsync(5);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Great stay!", result.Comment);
        }

        [Test]
        public async Task GetReviewsByHotelAsync_ValidHotelId_ReturnsReviews()
        {
            // Arrange
            var review = new Review { ReviewId = 6, Comment = "Great stay!", HotelId = 1, UserId = 1 };
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            var reviewsDto = new List<ReviewDTO> { new ReviewDTO { Comment = "Great stay!" } };
            _mockMapper.Setup(m => m.Map<IEnumerable<ReviewDTO>>(It.IsAny<IEnumerable<Review>>())).Returns(reviewsDto);

            // Act
            var result = await _reviewService.GetReviewsByHotelAsync(1);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Great stay!", result.First().Comment);
        }

        [Test]
        public async Task GetReviewsByUserAsync_ValidUserId_ReturnsReviews()
        {
            // Arrange
            var review = new Review { ReviewId = 7, Comment = "Great stay!", HotelId = 1, UserId = 1 };
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            var reviewsDto = new List<ReviewDTO> { new ReviewDTO {Comment = "Great stay!" } };
            _mockMapper.Setup(m => m.Map<IEnumerable<ReviewDTO>>(It.IsAny<IEnumerable<Review>>())).Returns(reviewsDto);

            // Act
            var result = await _reviewService.GetReviewsByUserAsync(1);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Great stay!", result.First().Comment);
        }

        [Test]
        public async Task UpdateReviewAsync_ValidReview_ReturnsUpdatedReviewDTO()
        {
            // Arrange
            var review = new Review { ReviewId = 8, Comment = "Great stay!", HotelId = 1, UserId = 1 };
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            var updatedReviewDto = new ReviewDTO { Comment = "Amazing stay!" };
            _mockMapper.Setup(m => m.Map(updatedReviewDto, review)).Returns(review);
            _mockMapper.Setup(m => m.Map<ReviewDTO>(review)).Returns(updatedReviewDto);

            // Act
            var result = await _reviewService.UpdateReviewAsync(8, updatedReviewDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Amazing stay!", result.Comment);
        }

        [Test]
        public async Task UpdateReviewAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var updatedReviewDto = new ReviewDTO { Comment = "Amazing stay!" };

            // Act
            var result = await _reviewService.UpdateReviewAsync(999, updatedReviewDto);

            // Assert
            Assert.IsNull(result);
        }
    }
}
