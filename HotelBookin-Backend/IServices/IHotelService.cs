using HotelBookin_Backend.DTO;

namespace HotelBookin_Backend.IServices
{
    public interface IHotelService
    {
        Task<List<HotelDTO>> GetAllHotelsAsync();
        Task<HotelDTO> GetHotelByIdAsync(int id);
        Task<List<RoomDTO>> GetRoomsByHotelIdAsync(int hotelId);
        Task<HotelDTO> AddHotelAsync(HotelDTO hotel);
        Task<HotelDTO> UpdateHotelAsync(int id, HotelDTO hotel);
        Task<bool> DeleteHotelAsync(int id);
    }
}
