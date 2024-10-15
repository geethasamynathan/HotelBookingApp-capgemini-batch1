using HotelBookin_Backend.DTO;

namespace HotelBookin_Backend.IServices
{
    public interface IPaymentService
    {
        Task<PaymentDTO> ProcessPayment(ProcessPaymentDTO processPaymentDto);
        Task<PaymentDTO> GetPaymentDetails(int id);
        Task<List<PaymentDTO>> GetUserPayments(int userId);
    }
}
