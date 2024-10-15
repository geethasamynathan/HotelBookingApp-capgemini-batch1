namespace HotelBookin_Backend.Custom_Exceptions
{
    /// <summary>
    /// Exception thrown when an invalid reservation status is provided.
    /// </summary>
    public class InvalidReservationStatusException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the InvalidReservationStatusException class.
        /// </summary>
        public InvalidReservationStatusException()
            : base("The provided reservation status is invalid.")
        {
        }
    }
}
