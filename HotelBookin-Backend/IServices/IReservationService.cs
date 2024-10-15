using HotelBookin_Backend.DTO;

namespace HotelBookin_Backend.IServices
{
    public interface IReservationService
    {
        Task<ReservationDTO> AddNewReservation(ReservationDTO reservation);
        Task<ReservationDTO> UpdateReservation(int id, UpdateReservationDto reservation);
        Task CancelReservation(int id);
        Task<IEnumerable<ReservationDTO>> GetAllReservation();
        Task<ReservationDTO> GetReservationById(int id);
        Task<List<ReservationDTO>> GetReservationByUserId(int userid);
        Task<List<ReservationDTO>> GetReservationByHotelId(int hotelid);


    }
}
