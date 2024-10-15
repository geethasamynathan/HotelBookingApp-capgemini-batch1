namespace HotelBookin_Backend.DTO
{
    public class ReservationDTO
    {
        public int UserId { get; set; }

        public int HotelId { get; set; }

        public int RoomId { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }
    }
}
