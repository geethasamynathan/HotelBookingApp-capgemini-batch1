namespace HotelBookin_Backend.DTO
{
    public class ProcessPaymentDTO
    {
        public int UserID { get; set; }
        public decimal Amount { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationDate { get; set; } // Format: MM/YY
        public string CVV { get; set; }

    }
}
