namespace HotelBookin_Backend.Custom_Exceptions
{
    /// <summary>
    /// Exception thrown when no rooms are available for booking.
    /// </summary>
    public class NoRoomsAvailableException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the NoRoomsAvailableException class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NoRoomsAvailableException(string message)
            : base(message)
        {
        }
    }
}
