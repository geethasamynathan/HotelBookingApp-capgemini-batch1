namespace HotelBookin_Backend.DTO
{
    public class PaymentDTO
    {
        public int PaymentID { get; set; }
        public decimal Amount { get; set; }
        public int UserId { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
    }
}
