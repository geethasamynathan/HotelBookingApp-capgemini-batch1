namespace HotelBookin_Backend.Custom_Exceptions
{
    /// <summary>
    /// Exception thrown when there is an error during payment processing.
    /// </summary>
    public class PaymentProcessingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the PaymentProcessingException class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public PaymentProcessingException(string message)
            : base(message)
        {
        }
    }
}
