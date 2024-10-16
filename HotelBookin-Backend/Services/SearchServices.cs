using AutoMapper;
using HotelBookin_Backend.Data;
using HotelBookin_Backend.DTO;
using HotelBookin_Backend.Custom_Exceptions;
using HotelBookin_Backend.IServices;
using HotelBookin_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookin_Backend.Services
{
    /// <summary>
    /// Service for searching hotels and available rooms.
    /// </summary>
    public class SearchServices : ISearchServices
    {
        private readonly HotelBookingDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchServices"/> class.
        /// </summary>
        /// <param name="mapper">Mapper for converting between Hotel and Room DTOs and their corresponding entities.</param>
        /// <param name="context">Database context for accessing hotel and room data.</param>
        public SearchServices(IMapper mapper, HotelBookingDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Searches for hotels by location asynchronously.
        /// </summary>
        /// <param name="location">The location to search for hotels.</param>
        /// <returns>A list of HotelDTO objects representing the hotels found.</returns>
        /// <exception> NoHotelsFoundException Thrown when no hotels match the search criteria.</exception>
        public async Task<List<HotelDTO>> SearchHotelsAsync(string location)
        {
            // Await the result from GetHotelsByLocationAsync to ensure it's completed
            var hotels = await _context.Hotels.Where(h => h.Location == location).ToListAsync();
            if (!hotels.Any())
            {
                throw new NoHotelsFoundException("No hotels found matching your criteria.");
            }

            return _mapper.Map<List<HotelDTO>>(hotels); // Return the filtered hotels

        }

        /// <summary>
        /// Retrieves available rooms for a given hotel within specified dates asynchronously.
        /// </summary>
        /// <param name="HotelId">The ID of the hotel to check for available rooms.</param>
        /// <param name="checkIn">The check-in date.</param>
        /// <param name="checkOut">The check-out date.</param>
        /// <returns>A list of RoomDTO objects representing the available rooms.</returns>
        /// <exception>NoRoomsAvailableException Thrown when no rooms are available for the selected dates.</exception>
        public async Task<List<RoomDTO>> GetAvailableRoomAsync(int HotelId, DateTime checkIn, DateTime checkOut)
        {
            var availableRooms = await _context.Rooms.Where(room =>
                    !_context.Reservations.Any(res =>
                        res.RoomId == room.RoomId &&
                        ((checkIn >= res.CheckInDate && checkIn < res.CheckOutDate) ||
                         (checkOut > res.CheckInDate && checkOut <= res.CheckOutDate) ||
                         (checkIn <= res.CheckInDate && checkOut >= res.CheckOutDate))
                    ) && room.Availability
                ).ToListAsync();

            if (!availableRooms.Any())
            {
                throw new NoRoomsAvailableException("No rooms available for the selected dates.");
            }

            return _mapper.Map<List<RoomDTO>>(availableRooms);

        }
    }
}
