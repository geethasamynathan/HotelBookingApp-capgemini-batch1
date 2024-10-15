using System.ComponentModel.DataAnnotations;

namespace HotelBookin_Backend.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username can't be longer than 50 characters.")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "Password must be at least 8 characters long and can't be longer than 100 characters.", MinimumLength = 6)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = null!;

        [StringLength(50, ErrorMessage = "First name can't be longer than 50 characters.")]
        public string? FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Last name can't be longer than 50 characters.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; } = null!;

        [StringLength(200, ErrorMessage = "Address can't be longer than 200 characters.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Role is required(customer,hotel_staff,admin).")]
        [StringLength(20, ErrorMessage = "Role can't be longer than 20 characters.")]
        public string Role { get; set; } = null!;//user,admin,hotel_staff

        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
