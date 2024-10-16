using AutoMapper;
using HotelBookin_Backend.Custom_Exceptions;
using HotelBookin_Backend.Data;
using HotelBookin_Backend.DTO;
using HotelBookin_Backend.Models;
using HotelBookin_Backend.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking_Backend_Service_TestProject
{
    internal class SearchServiceTests
    {
        private SearchServices _searchServices;
        private HotelBookingDbContext _context;
        private Mock<IMapper> _mockMapper;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<HotelBookingDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new HotelBookingDbContext(options);
            _mockMapper = new Mock<IMapper>();
            _searchServices = new SearchServices(_mockMapper.Object, _context);
        }

        [Test]
        public async Task SearchHotelsAsync_ValidLocation_ReturnsHotelDTOList()
        {
            // Arrange
            var hotel = new Hotel { HotelId = 11, ContactNumber = "4152637894", Location = "New York", HotelName = "Test" };
            await _context.Hotels.AddAsync(hotel);
            await _context.SaveChangesAsync();

            var hotelDto = new HotelDTO {  Location = "New York" };
            _mockMapper.Setup(m => m.Map<List<HotelDTO>>(It.IsAny<List<Hotel>>()))
                        .Returns(new List<HotelDTO> { hotelDto });

            // Act
            var result = await _searchServices.SearchHotelsAsync("New York");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("New York", result.First().Location);
        }

        [Test]
        public void SearchHotelsAsync_NoHotelsFound_ThrowsNoHotelsFoundException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<NoHotelsFoundException>(async () => await _searchServices.SearchHotelsAsync("Unknown Location"));
            Assert.AreEqual("No hotels found matching your criteria.", ex.Message);
        }

        [Test]
        public async Task GetAvailableRoomAsync_ValidHotelId_ReturnsAvailableRoomDTOList()
        {
            // Arrange
            var hotel = new Hotel { HotelId = 2, ContactNumber = "4152637894", Location = "Hyd", HotelName = "Test" };
            var room = new Room { RoomId = 2, HotelId = 1, Availability = true, RoomType = "single" };
            await _context.Hotels.AddAsync(hotel);
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();

            var roomDto = new RoomDTO { RoomId = 1, HotelId = 1 };
            _mockMapper.Setup(m => m.Map<List<RoomDTO>>(It.IsAny<List<Room>>()))
                        .Returns(new List<RoomDTO> { roomDto });

            // Act
            var result = await _searchServices.GetAvailableRoomAsync(1, DateTime.Now.AddDays(1), DateTime.Now.AddDays(3));

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public async Task GetAvailableRoomAsync_NoRoomsAvailable_ThrowsNoRoomsAvailableException()
        {
            // Arrange
            var hotel = new Hotel { HotelId = 1,ContactNumber="4152637894",Location="Hyd",HotelName="Test" };
            var room = new Room { RoomId = 1, HotelId = 1, Availability = true,RoomType="single" };
            var reservation = new Reservation { ReservationId = 1, RoomId = 1, CheckInDate = DateTime.Now.AddDays(1), CheckOutDate = DateTime.Now.AddDays(2),Status="confirmed" };

            await _context.Hotels.AddAsync(hotel);
            await _context.Rooms.AddAsync(room);
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();

            // Act & Assert
            var ex = Assert.ThrowsAsync<NoRoomsAvailableException>(async () =>
                await _searchServices.GetAvailableRoomAsync(1, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2)));
            Assert.AreEqual("No rooms available for the selected dates.", ex.Message);
        }
    }
}
