using System.ComponentModel.DataAnnotations;

namespace HotelBookin_Backend.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Hotel ID is required.")]
        public int HotelId { get; set; }

        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [StringLength(500, ErrorMessage = "Comment can't be longer than 500 characters.")]
        public string? Comment { get; set; }

        [Required(ErrorMessage = "Review date is required.")]
        public DateTime ReviewDate { get; set; } = DateTime.UtcNow; // Default to current date and time

        public virtual Hotel Hotel { get; set; } = null!;

        public virtual User User { get; set; } = null!;

    }
}
