using AutoMapper;
using HotelBookin_Backend.IServices;
using HotelBookin_Backend.DTO;
using HotelBookin_Backend.Custom_Exceptions;
using HotelBookin_Backend.Models;
using Microsoft.EntityFrameworkCore;
using HotelBookin_Backend.Data;

namespace HotelBookin_Backend.Services
{
    /// <summary>
    /// Service for managing room-related operations in the hotel booking system.
    /// </summary>
    public class RoomManagement : IRoomManagement
    {
        private readonly HotelBookingDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomManagement"/> class.
        /// </summary>
        /// <param name="mapper">Mapper for converting between Room and RoomDTO objects.</param>
        /// <param name="context">Database context for accessing room data.</param>
        public RoomManagement(IMapper mapper, HotelBookingDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        /// <summary>
        /// Adds a new room asynchronously.
        /// </summary>
        /// <param name="roomdto">The DTO containing room information.</param>
        /// <returns>A message indicating the result of the operation.</returns>
        /// <exception> InvalidRoomDataException Thrown when room data is null.</exception>
        public async Task<string> AddRoomAsync(RoomDTO roomdto)
        {
            try
            {
                if (roomdto == null)
                    throw new InvalidRoomDataException("Room data cannot be null.");

                // Map RoomDTO to Room entity
                var room = new Room
                {
                    RoomType = roomdto.RoomType,
                    Description = roomdto.Description,
                    Price = roomdto.Price,
                    Availability = roomdto.Availability,
                    HotelId = roomdto.HotelId
                };

                await _context.Rooms.AddAsync(room);
                await _context.SaveChangesAsync();
                return $"Room with ID {room.RoomId} added successfully.";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all rooms asynchronously.
        /// </summary>
        /// <returns>A list of RoomDTO objects representing all rooms.</returns>
        public async Task<List<RoomDTO>> GetAllRooms()
        {
            try
            {
                var rooms = await _context.Rooms.ToListAsync();
                return _mapper.Map<List<RoomDTO>>(rooms);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a room by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the room to retrieve.</param>
        /// <returns>RoomDTO object representing the room.</returns>
        /// <exception> RoomNotFoundException Thrown when the room is not found.</exception>
        public async Task<RoomDTO> GetRoomById(int id)
        {
            try
            {
                var room = await _context.Rooms.FindAsync(id);
                if (room == null)
                {
                    throw new RoomNotFoundException(id);
                }
                return _mapper.Map<RoomDTO>(room);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing room asynchronously.
        /// </summary>
        /// <param name="updateroom">The DTO containing updated room information.</param>
        /// <returns>A message indicating the result of the operation.</returns>
        /// <exception> InvalidRoomDataException Thrown when room data is null.</exception>
        /// <exception> RoomNotFoundException Thrown when the room is not found.</exception>
        public async Task<string> UpdateRoomAsync(RoomDTO updateroom)
        {
            try
            {
                if (updateroom == null)
                    throw new InvalidRoomDataException("Room data cannot be null.");

                var room = await _context.Rooms.FirstOrDefaultAsync(i => i.RoomId == updateroom.RoomId);
                if (room == null)
                    throw new RoomNotFoundException(updateroom.RoomId);

                room.Price = updateroom.Price;
                room.Availability = updateroom.Availability;
                room.Description = updateroom.Description;
                room.RoomType = updateroom.RoomType;

                await _context.SaveChangesAsync();
                return "Room updated successfully";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a room by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the room to delete.</param>
        /// <returns>A message indicating the result of the operation.</returns>
        /// <exception> RoomNotFoundException Thrown when the room is not found.</exception>
        public async Task<string> DeleteRoomAsync(int id)
        {
            try
            {
                var room = await _context.Rooms.FirstOrDefaultAsync(i => i.RoomId == id);
                if (room == null)
                    throw new RoomNotFoundException(id);

                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
                return "Room Deleted";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
