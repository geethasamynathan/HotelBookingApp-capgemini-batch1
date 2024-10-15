namespace HotelBookin_Backend.Custom_Exceptions
{
    /// <summary>
    /// Exception thrown when a room is not found.
    /// </summary>
    public class RoomNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the  RoomNotFoundException class.
        /// </summary>
        /// <param name="roomId">The ID of the room that was not found.</param>
        public RoomNotFoundException(int roomId)
            : base($"Room with ID {roomId} not found.") { }
    }

    /// <summary>
    /// Exception thrown when a specific room is not found in a hotel.
    /// </summary>
    public class HotelRoomNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the HotelRoomNotFoundException class.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel where the room was not found.</param>
        /// <param name="roomId">The ID of the room that was not found.</param>
        public HotelRoomNotFoundException(int hotelId, int roomId)
            : base($"Room with ID {roomId} not found in Hotel ID {hotelId}.") { }
    }

    /// <summary>
    /// Exception thrown when the provided room data is invalid.
    /// </summary>
    public class InvalidRoomDataException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the InvalidRoomDataException class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public InvalidRoomDataException(string message) : base(message) { }
    }
}
