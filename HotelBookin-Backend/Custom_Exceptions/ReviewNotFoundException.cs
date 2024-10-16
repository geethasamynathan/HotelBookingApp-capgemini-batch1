using HotelBookin_Backend.Models;

namespace HotelBookin_Backend.Custom_Exceptions
{
    public class ReviewNotFoundException:Exception
    {
        public ReviewNotFoundException(int reviewId):base($"Review with ID {reviewId} not found.")
        {
                
        }
    }
}
