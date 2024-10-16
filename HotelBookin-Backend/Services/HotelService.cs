using AutoMapper;
using HotelBookin_Backend.Data;
using HotelBookin_Backend.DTO;
using HotelBookin_Backend.IServices;
using HotelBookin_Backend.Models;
using HotelBookin_Backend.Custom_Exceptions;
using Microsoft.EntityFrameworkCore;

namespace HotelBookin_Backend.Services
{
    /// <summary>
    /// Service for managing hotel-related operations.
    /// </summary>
    public class HotelService : IHotelService
    {
        private readonly HotelBookingDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the HotelService class.
        /// </summary>
        /// <param name="context">Database context for accessing hotel data.</param>
        /// <param name="mapper">Mapper for converting between Hotel and HotelDTO objects.</param>
        public HotelService(HotelBookingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all hotels asynchronously.
        /// </summary>
        /// <returns>A list of HotelDTO objects representing all hotels.</returns>
        public async Task<List<HotelDTO>> GetAllHotelsAsync()
        {
            try
            {
                var hotels = await _context.Hotels.ToListAsync();
                return _mapper.Map<List<HotelDTO>>(hotels);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a hotel by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the hotel to retrieve.</param>
        /// <returns> HotelDTO object representing the hotel.</returns>
        /// <exception >HotelNotFoundException Thrown when a hotel with the specified ID is not found.</exception>
        public async Task<HotelDTO> GetHotelByIdAsync(int id)
        {
            var hotel = await _context.Hotels.FirstOrDefaultAsync(h => h.HotelId == id);
            if (hotel == null)
                throw new HotelNotFoundException(id);
            return _mapper.Map<HotelDTO>(hotel);
        }

        /// <summary>
        /// Adds a new hotel asynchronously.
        /// </summary>
        /// <param name="hotelDto">The DTO containing hotel information to be added.</param>
        /// <returns> HotelDTO object representing the added hotel.</returns>
        public async Task<HotelDTO> AddHotelAsync(HotelDTO hotelDto)
        {
            try
            {
                var hotel = _mapper.Map<Hotel>(hotelDto);
                _context.Hotels.Add(hotel);
                await _context.SaveChangesAsync();
                return _mapper.Map<HotelDTO>(hotel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing hotel asynchronously.
        /// </summary>
        /// <param name="id">The ID of the hotel to update.</param>
        /// <param name="hotelDto">The DTO containing updated hotel information.</param>
        /// <returns>HotelDTO object representing the updated hotel.</returns>
        /// <exception cref="HotelNotFoundException">Thrown when a hotel with the specified ID is not found.</exception>
        public async Task<HotelDTO> UpdateHotelAsync(int id, HotelDTO hotelDto)
        {
            try
            {
                var hotel = await _context.Hotels.FindAsync(id);
                if (hotel == null) throw new HotelNotFoundException(id);
                _mapper.Map(hotelDto, hotel);
                await _context.SaveChangesAsync();
                return _mapper.Map<HotelDTO>(hotel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a hotel by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the hotel to delete.</param>
        /// <returns>True if the hotel was successfully deleted; otherwise, false.</returns>
        /// <exception cref="HotelNotFoundException">Thrown when a hotel with the specified ID is not found.</exception>
        public async Task<bool> DeleteHotelAsync(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null) throw new HotelNotFoundException(id);
            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Retrieves all rooms associated with a specific hotel asynchronously.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel to retrieve rooms from.</param>
        /// <returns>A list of RoomDTO objects representing the rooms.</returns>
        public async Task<List<RoomDTO>> GetRoomsByHotelIdAsync(int hotelId)
        {
            try
            {
                var rooms = await _context.Rooms.Where(r => r.HotelId == hotelId).ToListAsync();
                return _mapper.Map<List<RoomDTO>>(rooms);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
