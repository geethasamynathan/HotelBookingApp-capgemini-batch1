using System.ComponentModel.DataAnnotations;

namespace HotelBookin_Backend.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(20, ErrorMessage = "Status length can't be more than 20 characters.")]
        public string Status { get; set; } // e.g., Completed, Pending, Failed

        [StringLength(50, ErrorMessage = "Payment method length can't be more than 50 characters.")]
        public string PaymentMethod { get; set; } // e.g., "Credit Card"

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }

    }
}
