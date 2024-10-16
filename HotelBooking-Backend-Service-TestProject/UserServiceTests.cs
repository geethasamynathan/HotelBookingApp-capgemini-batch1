using AutoMapper;
using HotelBookin_Backend.Custom_Exceptions;
using HotelBookin_Backend.Data;
using HotelBookin_Backend.DTO;
using HotelBookin_Backend.Models;
using HotelBookin_Backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
namespace HotelBooking_Backend_Service_TestProject
{
    [TestFixture]
    [Author("Kavya")]
    public class UserServiceTests
    {
        private UserService _userService;
        private HotelBookingDbContext _context;
        private IMapper _mapper;
        private Mock<IConfiguration> _configurationMock;

        [SetUp]
        public void Setup()
        {
            // Set up the in-memory database
            var options = new DbContextOptionsBuilder<HotelBookingDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new HotelBookingDbContext(options);

            // Initialize AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>().ReverseMap();
                cfg.CreateMap<UserRegisterDTO, User>().ReverseMap();
            });
            _mapper = config.CreateMapper();
            // Mock configuration for JWT settings
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(m => m["Jwt:Key"]).Returns("YourSuperSecretKeyIsConvertToHashCode123");
            _configurationMock.Setup(m => m["Jwt:Issuer"]).Returns("YourIssuer");
            _configurationMock.Setup(m => m["Jwt:Audience"]).Returns("YourAudience");

            // Initialize UserService
            _userService = new UserService(_mapper, _context, _configurationMock.Object);
        }

        [Test]
        public async Task RegisterUser_ValidData_ReturnsUserDTO()
        {
            // Arrange
            var userRegisterDto = new UserRegisterDTO
            {
                Email = "test@example.com",
                Password = "Password123",
                Username = "TestUser",
                PhoneNumber = "123-456-7890", // Added PhoneNumber
                Role = "User" // Added Role
            };

            // Act
            var result = await _userService.RegisterUser(userRegisterDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("test@example.com", result.Email);
            Assert.AreEqual("TestUser", result.Username);
            Assert.AreEqual("123-456-7890", result.PhoneNumber); // Assert PhoneNumber
            Assert.AreEqual("User", result.Role); // Assert Role
        }

        [Test]
        public async Task LoginUser_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                Email = "test@example.com",
                Password = "Password123", // Use a simple password for testing; should ideally be hashed
                Username = "TestUser",
                PhoneNumber = "123-456-7890", // Added PhoneNumber
                Role = "User" // Added Role
            };

            // You would normally hash the password when saving to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userLoginDto = new UserLoginDTO
            {
                Email = "test@example.com",
                Password = "Password123"
            };

            // Act
            var token = await _userService.LoginUser(userLoginDto);

            // Assert
            Assert.IsNotNull(token);
        }

        [Test]
        public async Task GetUserProfile_ExistingUserId_ReturnsUserDTO()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                Email = "test@example.com",
                Password = "Password123",
                Username = "TestUser",
                PhoneNumber = "123-456-7890", // Added PhoneNumber
                Role = "User" // Added Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userService.GetUserProfile(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("test@example.com", result.Email);
            Assert.AreEqual("123-456-7890", result.PhoneNumber); // Assert PhoneNumber
            Assert.AreEqual("User", result.Role); // Assert Role
        }

        [Test]
        public async Task UpdateUserProfile_ValidData_ReturnsUpdatedUserDTO()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                Email = "test@example.com",
                Password = "Password123",
                Username = "TestUser",
                PhoneNumber = "123-456-7890", // Added PhoneNumber
                Role = "User" // Added Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userProfileDto = new UserDTO
            {
                UserId = 1,
                Email = "updated@example.com",
                Username = "UpdatedUser",
                PhoneNumber = "098-765-4321", // Updated PhoneNumber
                Role = "Admin" // Updated Role
            };

            // Act
            var result = await _userService.UpdateUserProfile(userProfileDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("updated@example.com", result.Email);
            Assert.AreEqual("UpdatedUser", result.Username);
            Assert.AreEqual("098-765-4321", result.PhoneNumber); // Assert updated PhoneNumber
            Assert.AreEqual("Admin", result.Role); // Assert updated Role
        }

        [Test]
        public async Task UpdateUserProfile_NonExistingUserId_ThrowsUserNotFoundException()
        {
            // Arrange
            var userProfileDto = new UserDTO
            {
                UserId = 999,
                Email = "updated@example.com",
                Username = "UpdatedUser",
                PhoneNumber = "098-765-4321", // Added PhoneNumber
                Role = "Admin" // Added Role
            };

            // Act & Assert
            Assert.ThrowsAsync<UserNotFoundException>(async () => await _userService.UpdateUserProfile(userProfileDto));
        }
    }

}