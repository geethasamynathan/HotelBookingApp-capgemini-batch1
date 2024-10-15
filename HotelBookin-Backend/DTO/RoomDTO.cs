namespace HotelBookin_Backend.DTO
{
    public class RoomDTO
    {
        public int RoomId { get; set; }
        public int HotelId { get; set; }

        public string RoomType { get; set; } = null!;

        public decimal Price { get; set; }

        public bool Availability { get; set; }

        public string? Description { get; set; }


    }
}
