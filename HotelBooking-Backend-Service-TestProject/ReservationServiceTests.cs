using AutoMapper;
using HotelBookin_Backend.Custom_Exceptions;
using HotelBookin_Backend.Data;
using HotelBookin_Backend.DTO;
using HotelBookin_Backend.Models;
using HotelBookin_Backend.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking_Backend_Service_TestProject
{
    [TestFixture]
    public class ReservationServiceTests
    {
        private ReservationService _reservationService;
        private Mock<IMapper> _mockMapper;
        private HotelBookingDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<HotelBookingDbContext>()
                .UseInMemoryDatabase(databaseName: "HotelBooking_DB")
                .Options;

            _context = new HotelBookingDbContext(options);
            _mockMapper = new Mock<IMapper>();
            _reservationService = new ReservationService(_context, _mockMapper.Object);
        }

        [Test]
        public async Task AddNewReservation_ValidReservation_ReturnsReservationDTO()
        {
            // Arrange
            var reservationDto = new ReservationDTO { HotelId = 1, UserId = 1 };
            var reservation = new Reservation { ReservationId = 1, HotelId = 1, UserId = 1, Status = "confirmed" };

            _mockMapper.Setup(m => m.Map<Reservation>(It.IsAny<ReservationDTO>()))
                        .Returns(reservation);
            _mockMapper.Setup(m => m.Map<ReservationDTO>(It.IsAny<Reservation>()))
                        .Returns(reservationDto);

            // Act
            var result = await _reservationService.AddNewReservation(reservationDto);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task CancelReservation_ExistingReservation_UpdatesStatus()
        {
            // Arrange
            var reservation = new Reservation { ReservationId = 3, Status = "confirmed" };
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();

            // Act
            await _reservationService.CancelReservation(reservation.ReservationId);

            // Assert
            var updatedReservation = await _context.Reservations.FindAsync(reservation.ReservationId);
            Assert.IsNotNull(updatedReservation);
            Assert.AreEqual("Cancelled", updatedReservation.Status);
        }

        [Test]
        public void CancelReservation_NonExistingReservation_ThrowsReservationNotFoundException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ReservationNotFoundException>(async () => await _reservationService.CancelReservation(111));
            Assert.AreEqual($"Reservation with ID 111 not found.", ex.Message);
        }

        [Test]
        public async Task GetAllReservation_ReturnsListOfReservationDTOs()
        {
            // Arrange
            var reservations = new List<Reservation>
            {
                new Reservation { ReservationId = 4, Status = "confirmed" },
                new Reservation { ReservationId = 5, Status = "confirmed" }
            };

            await _context.Reservations.AddRangeAsync(reservations);
            await _context.SaveChangesAsync();

            _mockMapper.Setup(m => m.Map<IEnumerable<ReservationDTO>>(It.IsAny<IEnumerable<Reservation>>()))
                        .Returns(new List<ReservationDTO>
                        {
                            new ReservationDTO { UserId = 1 },
                            new ReservationDTO {UserId = 2}
                        });

            // Act
            var result = await _reservationService.GetAllReservation();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetReservationById_ExistingReservation_ReturnsReservationDTO()
        {
            // Arrange
            var reservation = new Reservation { ReservationId = 6, Status = "confirmed" };
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();

            _mockMapper.Setup(m => m.Map<ReservationDTO>(It.IsAny<Reservation>()))
                        .Returns(new ReservationDTO {UserId = 1 });

            // Act
            var result = await _reservationService.GetReservationById(reservation.ReservationId);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetReservationById_NonExistingReservation_ThrowsReservationNotFoundException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ReservationNotFoundException>(async () => await _reservationService.GetReservationById(111));
            Assert.AreEqual("Reservation with ID 111 not found.", ex.Message);
        }

        [Test]
        public async Task UpdateReservation_ValidUpdate_ReturnsUpdatedReservationDTO()
        {
            // Arrange
            var reservation = new Reservation { ReservationId = 11, Status = "confirmed" };
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();

            var updateDto = new UpdateReservationDto { Status = "Cancelled" };

            _mockMapper.Setup(m => m.Map(It.IsAny<UpdateReservationDto>(), It.IsAny<Reservation>()))
                        .Callback<UpdateReservationDto, Reservation>((src, dest) => { dest.Status = src.Status; });
            _mockMapper.Setup(m => m.Map<ReservationDTO>(It.IsAny<Reservation>()))
                        .Returns(new ReservationDTO {UserId = 11 });

            // Act
            var result = await _reservationService.UpdateReservation(reservation.ReservationId, updateDto);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void UpdateReservation_NonExistingReservation_ThrowsReservationNotFoundException()
        {
            // Arrange
            var updateDto = new UpdateReservationDto { Status = "Cancelled" };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ReservationNotFoundException>(async () => await _reservationService.UpdateReservation(111, updateDto));
            Assert.AreEqual("Reservation with ID 111 not found.", ex.Message);
        }

        [Test]
        public void UpdateReservation_InvalidStatus_ThrowsInvalidReservationStatusException()
        {
            // Arrange
            var reservation = new Reservation { ReservationId = 1, Status = "confirmed" };
             _context.Reservations.AddAsync(reservation);
             _context.SaveChangesAsync();

            var updateDto = new UpdateReservationDto { Status = "InvalidStatus" };

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidReservationStatusException>(async () => await _reservationService.UpdateReservation(reservation.ReservationId, updateDto));
            Assert.IsNotNull(ex);
        }
    }
}
