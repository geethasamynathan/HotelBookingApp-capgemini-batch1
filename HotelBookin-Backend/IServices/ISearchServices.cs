using HotelBookin_Backend.DTO;

namespace HotelBookin_Backend.IServices
{
    public interface ISearchServices
    {
        Task<List<HotelDTO>> SearchHotelsAsync(string location);
        Task<List<RoomDTO>> GetAvailableRoomAsync(int HotelId,DateTime checkIn, DateTime checkOut);
    }
}
