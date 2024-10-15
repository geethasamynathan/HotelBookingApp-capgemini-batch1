using HotelBookin_Backend.DTO;
using HotelBookin_Backend.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookin_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _service;

        public ReviewsController(IReviewService service)
        {
            _service = service;
        }

        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetReviews()
        {
            var reviews = await _service.GetAllReviewsAsync();
            return Ok(reviews);
        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDTO>> GetReview(int id)
        {
            var review = await _service.GetReviewByIdAsync(id)
;
            if (review == null)
            {
                return NotFound();
            }
            return Ok(review);
        }

        // GET: api/Reviews/ByHotel/5
        [HttpGet("ByHotel/{hotelId}")]
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetReviewsByHotel(int hotelId)
        {
            var reviews = await _service.GetReviewsByHotelAsync(hotelId);
            if (reviews == null)
            {
                return NotFound();
            }
            return Ok(reviews);
        }

        // GET: api/Reviews/ByUser/5
        [HttpGet("ByUser/{userId}")]
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetReviewsByUser(int userId)
        {
            var reviews = await _service.GetReviewsByUserAsync(userId);
            if (reviews == null)
            {
                return NotFound();
            }
            return Ok(reviews);
        }

        // PUT: api/Reviews/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, ReviewDTO reviewDto)
        {
            if (reviewDto == null)
            {
                return BadRequest();
            }
            var updateReview = await _service.UpdateReviewAsync(id, reviewDto);
            if (updateReview == null)
            {
                return NotFound();
            }
            return Ok(updateReview);
        }

        // POST: api/Reviews
        [HttpPost]
        public async Task<ActionResult<ReviewDTO>> PostReview(ReviewDTO reviewDto)
        {
            var review = await _service.AddReviewAsync(reviewDto);
            if (review == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetReview), new { id = review.ReviewDate }, review);
        }

        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _service.GetReviewByIdAsync(id)
;
            if (review == null)
            {
                return NotFound();
            }
            await _service.DeleteReviewAsync(id)
;
            return NoContent();
        }

        private async Task<bool> ReviewExists(int id)
        {
            var review = await _service.GetReviewByIdAsync(id)
;
            return review != null;
        }
    }
}
