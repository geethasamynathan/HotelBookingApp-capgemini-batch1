namespace HotelBookin_Backend.Custom_Exceptions
{
    /// <summary>
    /// Exception thrown when a hotel is not found in the database.
    /// </summary>
    public class HotelNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the HotelNotFoundException class.
        /// </summary>
        /// <param name="id">The ID of the hotel that was not found.</param>
        public HotelNotFoundException(int id)
            //: base($"Hotel with ID {id} not found.")
        {
        }
    }
}
