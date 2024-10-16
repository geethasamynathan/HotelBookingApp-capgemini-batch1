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
    [TestFixture]
    [Author("kavya")]
    public class HotelServiceTests
    {
        private HotelService _hotelService;
        private Mock<IMapper> Mapper;
        private HotelBookingDbContext _context;

        [SetUp]
        public void Setup()
        {
            // Create in-memory database
            var options = new DbContextOptionsBuilder<HotelBookingDbContext>()
                .UseInMemoryDatabase(databaseName: "HotelBooking_DB")
                .Options;

            _context = new HotelBookingDbContext(options);
            Mapper = new Mock<IMapper>();
            _hotelService = new HotelService(_context, Mapper.Object);
        }


        [Test]
        public async Task GetAllHotelsAsync_ReturnsAllHotels()
        {
            // Arrange
            var hotels = new List<Hotel>
            {
                new Hotel { HotelId = 1,  HotelName = "Test Hotel1" ,ContactNumber="+91 9874512632",Location="HYd",Rating=3},
                new Hotel { HotelId = 2,  HotelName = "Test Hotel2" ,ContactNumber="+91 6302891716",Location="Pune",Rating=1}
            };
            
            _context.Hotels.AddRange(hotels);
            await _context.SaveChangesAsync();
            Mapper.Setup(m => m.Map<List<HotelDTO>>(It.IsAny<List<Hotel>>()))
                .Returns(new List<HotelDTO>
                {
                    new HotelDTO {HotelName = "Test Hotel", ContactNumber = "+91 9874512632", Location = "HYd", Rating = 3 },
                    new HotelDTO {HotelName = "Test Hotel2" ,ContactNumber="+91 6302891716",Location="Pune",Rating=1 }
                });
            // Act
            var result = await _hotelService.GetAllHotelsAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task GetHotelByIdAsync_ReturnsHotel()
        {
            // Arrange
            var hotel = new Hotel { HotelId = 21, HotelName = "Test Hotel", ContactNumber = "+91 9874512632", Location = "HYd", Rating = 3 };
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();
            Mapper.Setup(m => m.Map<HotelDTO>(It.IsAny<Hotel>()))
                .Returns(new HotelDTO { HotelName = "Test Hotel", ContactNumber = "+91 9874512632", Location = "HYd", Rating = 3 });
            // Act
            var result = await _hotelService.GetHotelByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(hotel.HotelName, result.HotelName);
        }

        [Test]
        public async Task GetHotelByIdAsync_ThrowsHotelNotFoundException_WhenHotelDoesNotExist()
        {
            // Arrange & Act & Assert
            Assert.ThrowsAsync<HotelNotFoundException>(async() =>await _hotelService.GetHotelByIdAsync(11111));
        }

        [Test]
        public async Task AddHotelAsync_AddsHotel()
        {
            // Arrange
            var hotel = new Hotel { HotelId = 1, HotelName = "Test Hotel", ContactNumber = "+91 9874512632", Location = "HYd", Rating = 3 };
            var hotelDto= new HotelDTO { HotelName = "Test Hotel", ContactNumber = "+91 9874512632", Location = "HYd", Rating = 3 };
            Mapper.Setup(m => m.Map<Hotel>(It.IsAny<HotelDTO>()))
                .Returns(hotel);

            Mapper.Setup(m => m.Map<HotelDTO>(It.IsAny<Hotel>()))
                .Returns(hotelDto);

            // Act
            var result = await _hotelService.AddHotelAsync(hotelDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(hotelDto.HotelName, result.HotelName);
            Assert.AreEqual(1, _context.Hotels.Count());
        }

        [Test]
        public async Task UpdateHotelAsync_UpdatesHotel()
        {
            // Arrange
            var hotel = new Hotel { HotelId = 16, HotelName = "demo Hotel", ContactNumber = "+91 9874512632", Location = "HYd", Rating = 3 };

            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();
            var hotelDto = new HotelDTO { HotelName = "Updated Name" };
            Mapper.Setup(m => m.Map(It.IsAny<HotelDTO>(), It.IsAny<Hotel>()))
                        .Callback<HotelDTO, Hotel>((src, dest) => { dest.HotelName = src.HotelName; });
            Mapper.Setup(m => m.Map<HotelDTO>(It.IsAny<Hotel>()))
                        .Returns(hotelDto);
            // Act
            var result = await _hotelService.UpdateHotelAsync(1, hotelDto);

            // Assert
            Assert.AreEqual(hotelDto.HotelName, result.HotelName); ;
        }

        [Test]
        public async Task DeleteHotelAsync_ExistingId_DeletesHotel()
        {
            // Arrange
            var hotel = new Hotel { HotelId = 10, HotelName = "Test Hotel", ContactNumber = "+91 9874512632", Location = "HYd", Rating = 3 };
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();

            // Act
            var result = await _hotelService.DeleteHotelAsync(1);

            // Assert
            Assert.True(result);
            Assert.AreEqual(1,_context.Hotels.Count());
        }
        [Test]
        public void DeleteHotelAsync_NonExistingId_ThrowsHotelNotFoundException()
        {
            // Act & Assert
            Assert.ThrowsAsync<HotelNotFoundException>(async () => await _hotelService.DeleteHotelAsync(111));
        }

        [Test]
        public async Task GetRoomsByHotelIdAsync_ReturnsRooms()
        {
            // Arrange
            var hotel = new Hotel { HotelId = 11, HotelName = "Test Hotel", ContactNumber = "+91 9874512632", Location = "HYd", Rating = 3 };
            var rooms = new List<Room>
            {
                new Room { RoomId = 1, HotelId = 11, RoomType = "Single" },
                new Room { RoomId = 2, HotelId = 11, RoomType = "Double" }
            };
            _context.Hotels.Add(hotel);
            _context.Rooms.AddRange(rooms);
            await _context.SaveChangesAsync();

            Mapper.Setup(m => m.Map<List<RoomDTO>>(It.IsAny<List<Room>>()))
                .Returns(new List<RoomDTO>
                {
                    new RoomDTO { RoomId = 1, HotelId = 1, RoomType = "Single" },
                    new RoomDTO { RoomId = 2, HotelId = 1, RoomType = "Double" }
                });

            // Act
            var result = await _hotelService.GetRoomsByHotelIdAsync(1);

            // Assert
            Assert.AreEqual(2, result.Count);
        }
    }
}
