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
    public class PaymentServiceTests
    {
        private HotelBookingDbContext _context;
        private PaymentService _paymentService;
        private Mock<IMapper> Mapper;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<HotelBookingDbContext>()
               .UseInMemoryDatabase(databaseName: "HotelBooking_DB")
               .Options;

            _context = new HotelBookingDbContext(options);
            Mapper = new Mock<IMapper>();
            _paymentService = new PaymentService(Mapper.Object, _context);

        }



        [Test]
        public async Task ProcessPayment_ValidCard_ReturnsPaymentDTO()
        {
            // Arrange
            var processPaymentDto = new ProcessPaymentDTO
            {
                UserID = 101,
                Amount = 100,
                CardNumber = "1234567812345678",
                ExpirationDate = "12/25",
                CVV = "123"
            };
            Mapper.Setup(m => m.Map<PaymentDTO>(It.IsAny<Payment>()))
                       .Returns(new PaymentDTO { Amount = processPaymentDto.Amount });

            // Act
            var result = await _paymentService.ProcessPayment(processPaymentDto);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task ProcessPayment_InvalidCard_ThrowsPaymentProcessingException()
        {
            // Arrange
            var processPaymentDto = new ProcessPaymentDTO
            {
                UserID = 1,
                Amount = 100,
                CardNumber = "1234",
                ExpirationDate = "12/25",
                CVV = "123"
            };

            // Act & Assert
            Assert.ThrowsAsync<PaymentProcessingException>(() => _paymentService.ProcessPayment(processPaymentDto));
        }

        [Test]
        public async Task GetPaymentDetails_ExistingPayment_ReturnsPaymentDTO()
        {
            // Arrange
            var payment = new Payment
            {
                PaymentId = 11,
                UserId = 1,
                Amount = 100,
                PaymentMethod = "Credit Card",
                Date = DateTime.Now,
                Status = "confirmed"
            };
            _context.Payments.Add(payment);
            _context.SaveChanges();
            Mapper.Setup(m => m.Map<PaymentDTO>(It.IsAny<Payment>()))
                       .Returns(new PaymentDTO { Amount = payment.Amount });

            // Act
            var result = await _paymentService.GetPaymentDetails(11);

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task GetPaymentDetails_NonExistentPayment_ThrowsPaymentNotFoundException()
        {
            // Arrange

            // Act & Assert
            Assert.ThrowsAsync<PaymentNotFoundException>(() => _paymentService.GetPaymentDetails(1));
        }

        [Test]
        public async Task GetUserPayments_ReturnsListOfPayments()
        {
            // Arrange
            var payment1 = new Payment { UserId = 1, Amount = 100, PaymentMethod = "Credit Card", Status = "confirmed" };
            var payment2 = new Payment { UserId = 1, Amount = 200, PaymentMethod = "Credit Card", Status = "confirmed" };
            await _context.Payments.AddRangeAsync(payment1, payment2);
            await _context.SaveChangesAsync();

            Mapper.Setup(m => m.Map<List<PaymentDTO>>(It.IsAny<List<Payment>>()))
                        .Returns(new List<PaymentDTO>
                        {
                            new PaymentDTO { Amount = payment1.Amount },
                            new PaymentDTO { Amount = payment2.Amount }
                        });

            // Act
            var result = await _paymentService.GetUserPayments(1);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
        }
    }
}
