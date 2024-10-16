using AutoMapper;
using HotelBookin_Backend.Custom_Exceptions;
using HotelBookin_Backend.Data;
using HotelBookin_Backend.DTO;
using HotelBookin_Backend.Models;
using HotelBookin_Backend.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking_Backend_Service_TestProject
{
    public class SearchServicesTest
    {
        private readonly HotelBookingDbContext _context;
        private readonly IMapper _mapper;
        private readonly SearchServices _searchServices;

        public SearchServicesTest()
        {
            // Set up the in-memory database
            var options = new DbContextOptionsBuilder<HotelBookingDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new HotelBookingDbContext(options);

            // Initialize AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Hotel, HotelDTO>().ReverseMap();
                cfg.CreateMap<Room, RoomDTO>().ReverseMap();
            });
            _mapper = config.CreateMapper();

            // Initialize SearchServices
            _searchServices = new SearchServices(_mapper, _context);
        }

        [Test]
        public async Task SearchHotelsAsync_ValidLocation_ReturnsHotels()
        {
            // Arrange
            var hotel = new Hotel { HotelId = 1, HotelName = "Hotel A", Location = "New York", ContactNumber = "1234567890" };
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();

            // Act
            var result = await _searchServices.SearchHotelsAsync("New York");

            // Assert
            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Hotel A", result.First().HotelName);
        }

       [Test]
public void SearchHotelsAsync_NoHotelsFound_ThrowsNoHotelsFoundException()
{
    // Act & Assert
    var ex = Assert.ThrowsAsync<NoHotelsFoundException>(async () =>
        await _searchServices.SearchHotelsAsync("Los Angeles"));

    Assert.AreEqual("No hotels found matching your criteria.", ex.Message);
}


        [Test]
        public async Task GetAvailableRoomAsync_ValidHotelAndDates_ReturnsAvailableRooms()
        {
            // Arrange
            var hotel = new Hotel { HotelId = 1, HotelName = "Hotel A", Location = "New York", ContactNumber = "1234567890" };
            var room = new Room { RoomId = 1, RoomType = "Standard", Availability = true, HotelId = 1 };
            _context.Hotels.Add(hotel);
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            // Act
            var result = await _searchServices.GetAvailableRoomAsync(1, DateTime.Now.AddDays(1), DateTime.Now.AddDays(3));

            // Assert
            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Standard", result.First().RoomType);
        }

        [Test]
        public async Task GetAvailableRoomAsync_NoRoomsAvailable_ThrowsNoRoomsAvailableException()
        {
            // Arrange
            var hotel = new Hotel { HotelId = 1, HotelName = "Hotel A", Location = "New York", ContactNumber = "1234567890", };
            var room = new Room { RoomId = 1, RoomType = "Standard", Availability = true, HotelId = 1 };
            var reservation = new Reservation { RoomId = 1, CheckInDate = DateTime.Now.AddDays(1), CheckOutDate = DateTime.Now.AddDays(3), Status = "confirmed" };
            _context.Hotels.Add(hotel);
            _context.Rooms.Add(room);
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            // Act & Assert
            var ex = Assert.ThrowsAsync<NoRoomsAvailableException>(async () =>
                await _searchServices.GetAvailableRoomAsync(1, DateTime.Now.AddDays(1), DateTime.Now.AddDays(3)));
            Assert.That(ex.Message, Is.EqualTo("No rooms available for the selected dates."));
        }
    }
}
