namespace HotelBookin_Backend.Custom_Exceptions
{
    /// <summary>
    /// Exception thrown when no hotels are found based on search criteria.
    /// </summary>
    public class NoHotelsFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the NoHotelsFoundException class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NoHotelsFoundException(string message)
            : base(message)
        {
        }
    }
}
