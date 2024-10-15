namespace HotelBookin_Backend.DTO
{
    public class ReviewDTO
    {
        public int UserId { get; set; }

        public int HotelId { get; set; }

        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime ReviewDate { get; set; }
    }
}
