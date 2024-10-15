namespace HotelBookin_Backend.Custom_Exceptions
{
    /// <summary>
    /// Exception thrown when a user is not found.
    /// </summary>
    public class UserNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the UserNotFoundException class.
        /// </summary>
        /// <param name="userId">The ID of the user that was not found.</param>
        public UserNotFoundException(int userId)
            : base($"User with ID {userId} not found.")
        {
        }
    }
}
