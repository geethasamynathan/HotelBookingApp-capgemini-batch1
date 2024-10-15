using HotelBookin_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookin_Backend.Data
{
    public class HotelBookingDbContext:DbContext
    {
        public HotelBookingDbContext(DbContextOptions<HotelBookingDbContext> options)
        : base(options)
        {
        }
        public virtual DbSet<Hotel> Hotels { get; set; }

        public virtual DbSet<Reservation> Reservations { get; set; }

        public virtual DbSet<Review> Reviews { get; set; }

        public virtual DbSet<Room> Rooms { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Payment> Payments { get; set; }

    }
}
