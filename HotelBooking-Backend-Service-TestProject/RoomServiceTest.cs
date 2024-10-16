using AutoMapper;
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
    [TestFixture]
    [Author("viji priya")]
    public class RoomServiceTest
    {
        private readonly HotelBookingDbContext _context;
        private readonly IMapper _mapper;
        private readonly RoomManagement _roommanagement;

        public RoomServiceTest()
        {
            // Set up the in-memory database
            var options = new DbContextOptionsBuilder<HotelBookingDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new HotelBookingDbContext(options);

            // Initialize AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Room, RoomDTO>().ReverseMap();
                cfg.CreateMap<Hotel, HotelDTO>().ReverseMap();
            });
            _mapper = config.CreateMapper();

            // Initialize RoomManagement service
            _roommanagement = new RoomManagement(_mapper, _context);

        }

        [Test]
        public async Task AddRoomAsync_ValidRoom_ShouldReturnSuccessMessage()
        {
            var roomDto = new RoomDTO
            {
                RoomType = "Deluxe",
                Description = "A deluxe room with sea view",
                Price = 1500,
                Availability = true,
                HotelId = 1
            };

            // Act
            var result = await _roommanagement.AddRoomAsync(roomDto);

            // Assert
            Assert.IsTrue(result.Contains("added successfully"));
            var roomInDb = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomType == "Deluxe");
            Assert.IsNotNull(roomInDb);
            Assert.That(roomInDb.Description, Is.EqualTo("A deluxe room with sea view"));
        }

        [Test]
        public async Task GetAllRooms_NoRooms_ReturnsEmptyList()
        {
            // Act
            var result = await _roommanagement.GetAllRooms();

            // Assert
            Assert.IsEmpty(result);
        }
        [Test]
        public async Task UpdateRoomAsync_ValidUpdate_ReturnsSuccessMessage()
        {
            // Arrange
            var room = new Room { RoomType = "Standard", Description = "Standard room.", Price = 100.00m, Availability = true, HotelId = 1 };
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            var updateDto = new RoomDTO
            {
                RoomId = room.RoomId,
                RoomType = "Deluxe",
                Description = "Upgraded to deluxe room.",
                Price = 150.00m,
                Availability = false,
                HotelId = 1
            };

            // Act
            var result = await _roommanagement.UpdateRoomAsync(updateDto);

            // Assert
            Assert.That(result, Is.EqualTo("Room updated successfully"));
            var updatedRoom = await _context.Rooms.FindAsync(room.RoomId);
            Assert.That(updatedRoom.RoomType, Is.EqualTo("Deluxe"));
            Assert.That(updatedRoom.Description, Is.EqualTo("Upgraded to deluxe room."));
            Assert.That(updatedRoom.Price, Is.EqualTo(150.00m));
            Assert.False(updatedRoom.Availability);
        }
        [Test]
        public async Task GetRoomById_ExistingId_ReturnsRoomDto()
        {
            // Arrange
            var room = new Room { RoomType = "Standard", Description = "Standard room.", Price = 100.00m, Availability = true, HotelId = 1 };
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            // Act
            var result = await _roommanagement.GetRoomById(room.RoomId);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.RoomType, Is.EqualTo("Standard"));
            Assert.That(result.Description, Is.EqualTo("Standard room."));
            Assert.That(result.Price, Is.EqualTo(100.00m));
            Assert.That(result.Availability, Is.True);
            Assert.That(result.HotelId, Is.EqualTo(1));
        }
        [Test]
        public async Task DeleteRoomAsync_ExistingId_ReturnsSuccessMessage()
        {
            // Arrange
            var room = new Room { RoomType = "Standard", Description = "Standard room.", Price = 100.00m, Availability = true, HotelId = 1 };
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            // Act
            var result = await _roommanagement.DeleteRoomAsync(room.RoomId);

            // Assert
            Assert.That(result, Is.EqualTo("Room Deleted"));
            var deletedRoom = await _context.Rooms.FindAsync(room.RoomId);
            Assert.Null(deletedRoom);
        }
        [Test]
        public async Task GetAllRooms_WithRooms_ReturnsAllRooms()
        {
            // Arrange
            var rooms = new List<Room>
    {
        new Room { RoomType = "Standard", Description = "Standard room.", Price = 100.00m, Availability = true, HotelId = 1 },
        new Room { RoomType = "Suite", Description = "Luxury suite.", Price = 300.00m, Availability = false, HotelId = 1 }
    };
            await _context.Rooms.AddRangeAsync(rooms);
            await _context.SaveChangesAsync();

            // Act
            var result = await _roommanagement.GetAllRooms();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));

            bool standardRoomExists = false;
            bool suiteRoomExists = false;

            foreach (var room in result)
            {
                if (room.RoomType == "Standard")
                {
                    standardRoomExists = true;
                }
                if (room.RoomType == "Suite")
                {
                    suiteRoomExists = true;
                }
            }

            Assert.True(standardRoomExists, "Standard room not found.");
            Assert.True(suiteRoomExists, "Suite room not found.");
        }

    }
}
