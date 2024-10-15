namespace HotelBookin_Backend.Custom_Exceptions

{
    /// <summary>
    /// Exception thrown when user login credentials are invalid.
    /// </summary>
    public class InvalidCredentialsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the InvalidCredentialsException class.
        /// </summary>
        public InvalidCredentialsException()
            : base("Invalid username or password.")
        {
        }
    }
}
