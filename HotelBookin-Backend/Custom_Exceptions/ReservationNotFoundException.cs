namespace HotelBookin_Backend.Custom_Exceptions
{
    /// <summary>
    /// Exception thrown when a reservation is not found.
    /// </summary>
    public class ReservationNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the ReservationNotFoundException class.
        /// </summary>
        /// <param name="reservationId">The ID of the reservation that was not found.</param>
        public ReservationNotFoundException(int reservationId)
            : base($"Reservation with ID {reservationId} not found.")
        {
        }
    }
}
