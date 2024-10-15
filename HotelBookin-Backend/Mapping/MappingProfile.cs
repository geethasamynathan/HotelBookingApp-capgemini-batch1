using AutoMapper;
using HotelBookin_Backend.DTO;
using HotelBookin_Backend.Models;

namespace HotelBooking_Backend.Mapping
{
    /// <summary>
    /// Defines the mapping configurations between the entity models and their corresponding DTOs.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile()
        {
            // Map User to UserDTO and vice versa
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserRegisterDTO>().ReverseMap();
            CreateMap<User, UserLoginDTO>().ReverseMap();

            // Map Hotel to HotelDTO and vice versa
            CreateMap<Hotel, HotelDTO>().ReverseMap();

            // Map Reservation to ReservationDTO and the update DTO
            CreateMap<Reservation, ReservationDTO>().ReverseMap();
            CreateMap<Reservation, UpdateReservationDto>().ReverseMap();

            // Map Room to RoomDTO and vice versa
            CreateMap<Room, RoomDTO>().ReverseMap();

            // Map Review to ReviewDTO and vice versa
            CreateMap<Review, ReviewDTO>().ReverseMap();

            // Map Payment to PaymentDTO and vice versa
            CreateMap<Payment, PaymentDTO>().ReverseMap();
        }
    }
}
