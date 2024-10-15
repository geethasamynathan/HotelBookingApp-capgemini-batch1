
using HotelBookin_Backend.DTO;

namespace HotelBookin_Backend.IServices
{
    public interface IRoomManagement
    {
        Task<List<RoomDTO>> GetAllRooms();
        Task<RoomDTO> GetRoomById(int id);
        Task<string> AddRoomAsync(RoomDTO room);
        Task<string> UpdateRoomAsync(RoomDTO room);
        Task<string> DeleteRoomAsync(int id);
       
    }
}
