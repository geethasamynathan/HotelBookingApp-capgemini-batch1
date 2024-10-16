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
    /// Service for managing reservations related operations.
    /// </summary>
    public class ReservationService : IReservationService
    {
        private readonly HotelBookingDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the ReservationService class.
        /// </summary>
        /// <param name="context">Database context for accessing reservation data.</param>
        /// <param name="mapper">Mapper for converting between Reservation and ReservationDTO objects.</param>
        public ReservationService(HotelBookingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds a new reservation asynchronously.
        /// </summary>
        /// <param name="reservationdto">The DTO containing reservation information.</param>
        /// <returns>ReservationDTO  object representing the added reservation.</returns>
        public async Task<ReservationDTO> AddNewReservation(ReservationDTO reservationdto)
        {
            try
            {
                var reservation = _mapper.Map<Reservation>(reservationdto);
                reservation.Status = "confirmed"; // Default status
                await _context.Reservations.AddAsync(reservation);
                await _context.SaveChangesAsync();
                return _mapper.Map<ReservationDTO>(reservation);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Cancels an existing reservation by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the reservation to cancel.</param>
        /// <exception> ReservationNotFoundException Thrown when the reservation is not found.</exception>
        public async Task CancelReservation(int id)
        {

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                throw new ReservationNotFoundException(id);
            }
            reservation.Status = "Cancelled"; // Update status
            await _context.SaveChangesAsync();

        }

        /// <summary>
        /// Retrieves all reservations asynchronously.
        /// </summary>
        /// <returns>A collection of ReservationDTO objects representing all reservations.</returns>
        public async Task<IEnumerable<ReservationDTO>> GetAllReservation()
        {
            try
            {
                var reservations = await _context.Reservations.ToListAsync();
                return _mapper.Map<IEnumerable<ReservationDTO>>(reservations);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all reservations for a specific hotel by its ID asynchronously.
        /// </summary>
        /// <param name="hotelid">The ID of the hotel to retrieve reservations for.</param>
        /// <returns>A list of ReservationDTO objects representing the hotel's reservations.</returns>
        public async Task<List<ReservationDTO>> GetReservationByHotelId(int hotelid)
        {
            try
            {
                var reservations = await _context.Reservations
                    .Where(r => r.HotelId == hotelid)
                    .ToListAsync();
                return _mapper.Map<List<ReservationDTO>>(reservations);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a reservation by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the reservation to retrieve.</param>
        /// <returns>A ReservationDTO object representing the reservation.</returns>
        /// <exception>ReservationNotFoundException Thrown when the reservation is not found.</exception>
        public async Task<ReservationDTO> GetReservationById(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                throw new ReservationNotFoundException(id);
            }
            return _mapper.Map<ReservationDTO>(reservation);

        }

        /// <summary>
        /// Retrieves all reservations made by a specific user by their ID asynchronously.
        /// </summary>
        /// <param name="userid">The ID of the user to retrieve reservations for.</param>
        /// <returns>A list of ReservationDTO objects representing the user's reservations.</returns>
        public async Task<List<ReservationDTO>> GetReservationByUserId(int userid)
        {
            try
            {
                var reservations = await _context.Reservations
                    .Where(r => r.UserId == userid)
                    .ToListAsync();
                return _mapper.Map<List<ReservationDTO>>(reservations);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Validates the status of a reservation.
        /// </summary>
        /// <param name="status">The status to validate.</param>
        /// <returns>True if the status is valid; otherwise, false.</returns>
        private bool IsValidStatus(string status) => status == "confirmed" || status == "Cancelled"; // Add more statuses as needed

        /// <summary>
        /// Updates an existing reservation by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the reservation to update.</param>
        /// <param name="reservation">The DTO containing updated reservation information.</param>
        /// <returns>ReservationDTO  object representing the updated reservation.</returns>
        /// <exception> ReservationNotFoundException Thrown when the reservation is not found.</exception>
        /// <exception> InvalidReservationStatusException Thrown when the provided status is invalid.</exception>
        public async Task<ReservationDTO> UpdateReservation(int id, UpdateReservationDto reservation)
        {
            var existingReservation = await _context.Reservations.FindAsync(id);
            if (existingReservation == null) throw new ReservationNotFoundException(id);

            // Validate status if needed
            if (!IsValidStatus(reservation.Status))
            {
                throw new InvalidReservationStatusException();
            }

            _mapper.Map(reservation, existingReservation);
            await _context.SaveChangesAsync();
            return _mapper.Map<ReservationDTO>(existingReservation);

        }
    }
}
