namespace HotelBookin_Backend.Custom_Exceptions
{
    /// <summary>
    /// Exception thrown when a payment is not found.
    /// </summary>
    public class PaymentNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the PaymentNotFoundException class.
        /// </summary>
        /// <param name="paymentId">The ID of the payment that was not found.</param>
        public PaymentNotFoundException(int paymentId)
            : base($"Payment with ID {paymentId} not found.")
        {
        }
    }
}
