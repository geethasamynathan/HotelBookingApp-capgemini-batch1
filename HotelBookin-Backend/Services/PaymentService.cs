using AutoMapper;
using HotelBookin_Backend.Custom_Exceptions;
using HotelBookin_Backend.Data;
using HotelBookin_Backend.DTO;
using HotelBookin_Backend.IServices;
using HotelBookin_Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HotelBookin_Backend.Services
{
    /// <summary>
    /// Service for managing payment-related operations.
    /// </summary>
    public class PaymentService : IPaymentService
    {
        private readonly IMapper _mapper;
        private readonly HotelBookingDbContext _context;

        /// <summary>
        /// Initializes a new instance of the PaymentService class.
        /// </summary>
        /// <param name="mapper">Mapper for converting between Payment and PaymentDTO objects.</param>
        /// <param name="context">Database context for accessing payment data.</param>
        public PaymentService(IMapper mapper, HotelBookingDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        /// <summary>
        /// Processes a payment asynchronously.
        /// </summary>
        /// <param name="processPaymentDto">DTO containing payment information to be processed.</param>
        /// <returns>PaymentDTO object representing the processed payment.</returns>
        /// <exception> PaymentProcessingException Thrown when card details are invalid.</exception>
        public async Task<PaymentDTO> ProcessPayment(ProcessPaymentDTO processPaymentDto)
        {
            try
            {
                // Validate card details (basic validation)
                if (!IsValidCard(processPaymentDto.CardNumber, processPaymentDto.ExpirationDate, processPaymentDto.CVV))
                {
                    throw new PaymentProcessingException("Invalid card details.");
                }

                // Create a new payment record
                var payment = new Payment
                {
                    UserId = processPaymentDto.UserID,
                    Amount = processPaymentDto.Amount,
                    PaymentMethod = "Credit Card",
                    Date = DateTime.Now,
                    Status = "confirmed" // In a real scenario, you might check payment gateway status
                };

                // Add payment to the database
                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();

                // Map the payment entity to a DTO and return
                return _mapper.Map<PaymentDTO>(payment);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves payment details by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the payment to retrieve.</param>
        /// <returns>PaymentDTO object representing the payment.</returns>
        /// <exception> PaymentNotFoundException Thrown when a payment with the specified ID is not found.</exception>
        public async Task<PaymentDTO> GetPaymentDetails(int id)
        {
            try
            {
                // Find the payment by ID
                var payment = await _context.Payments.FindAsync(id);
                if (payment == null)
                {
                    throw new PaymentNotFoundException(id);
                }

                // Map the payment entity to a DTO and return
                return _mapper.Map<PaymentDTO>(payment);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all payments made by a specific user asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve payments for.</param>
        /// <returns>A list of PaymentDTO objects representing the user's payments.</returns>
        public async Task<List<PaymentDTO>> GetUserPayments(int userId)
        {
            try
            {
                // Fetch all payments associated with the specified user ID
                var payments = await _context.Payments
                    .Where(p => p.UserId == userId)
                    .ToListAsync();
                var payment = await _context.Payments.FindAsync (userId);
                if(payment == null)throw new UserNotFoundException(userId);
                // Map the payment entities to DTOs and return
                return _mapper.Map<List<PaymentDTO>>(payments);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Validates credit card details.
        /// </summary>
        /// <param name="cardNumber">The credit card number.</param>
        /// <param name="expirationDate">The credit card expiration date in MM/yy format.</param>
        /// <param name="cvv">The credit card CVV.</param>
        /// <returns>True if the card details are valid; otherwise, false.</returns>
        private bool IsValidCard(string cardNumber, string expirationDate, string cvv)
        {
            // Simple card validation logic (you can enhance this)
            return cardNumber.Length == 16 &&
                   DateTime.TryParseExact(expirationDate, "MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var expiryDate) && expiryDate > DateTime.Now
                   && cvv.Length == 3;
        }
    }
}
