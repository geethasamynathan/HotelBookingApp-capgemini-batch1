using AutoMapper;
using HotelBookin_Backend.Custom_Exceptions;
using HotelBookin_Backend.DTO;
using HotelBookin_Backend.IServices;
using HotelBookin_Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HotelBookin_Backend.Data;

namespace HotelBookin_Backend.Services
{
    /// <summary>
    /// Service for user-related operations such as registration, login, and profile management.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly HotelBookingDbContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="mapper">Mapper for converting between User DTOs and their corresponding entities.</param>
        /// <param name="context">Database context for accessing user data.</param>
        /// <param name="configuration">Configuration for JWT settings.</param>
        public UserService(IMapper mapper, HotelBookingDbContext context, IConfiguration configuration)
        {
            _mapper = mapper;
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Registers a new user asynchronously.
        /// </summary>
        /// <param name="userRegisterDto">The user registration data transfer object.</param>
        /// <returns>UserDTO representing the registered user.</returns>
        public async Task<UserDTO> RegisterUser(UserRegisterDTO userRegisterDto)
        {
            try
            {
                var user = _mapper.Map<User>(userRegisterDto);
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Logs in a user and returns a JWT token.
        /// </summary>
        /// <param name="userLoginDto">The user login data transfer object.</param>
        /// <returns>A JWT token as a string.</returns>
        /// <exception> InvalidCredentialsException Thrown when login credentials are invalid.</exception>
        public async Task<string> LoginUser(UserLoginDTO userLoginDto)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == userLoginDto.Email);

                if (user == null || !VerifyPassword(userLoginDto.Password, user.Password))
                {
                    throw new InvalidCredentialsException();
                }

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddHours(5),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Retrieves the profile of a user by their ID asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>UserDTO representing the user profile.</returns>
        /// <exception>UserNotFoundException Thrown when the user is not found.</exception>
        public async Task<UserDTO> GetUserProfile(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new UserNotFoundException(userId);
                }
                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates the profile of a user asynchronously.
        /// </summary>
        /// <param name="userProfileDto">The user profile data transfer object.</param>
        /// <returns>UserDTO representing the updated user profile.</returns>
        /// <exception> UserNotFoundException Thrown when the user is not found.</exception>
        public async Task<UserDTO> UpdateUserProfile(UserDTO userProfileDto)
        {
            var user = await _context.Users.FindAsync(userProfileDto.UserId);
            if (user == null)
            {
                throw new UserNotFoundException(userProfileDto.UserId);
            }
            _mapper.Map(userProfileDto, user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserDTO>(user);
        }

        /// <summary>
        /// Logs out a user (token invalidation logic can be added here).
        /// </summary>
        /// <param name="userId">The ID of the user to log out.</param>
        public async Task LogoutUser(int userId)
        {
            // Handle user logout logic (e.g., token invalidation)
        }

        /// <summary>
        /// Verifies the password against the stored password.
        /// </summary>
        /// <param name="inputPassword">The password entered by the user.</param>
        /// <param name="storedPassword">The stored password (hashed).</param>
        /// <returns>True if the password matches; otherwise, false.</returns>
        private bool VerifyPassword(string inputPassword, string storedPassword)
        {
            // Implement password verification logic here
            return inputPassword == storedPassword; // Simplified, use hashing
        }
    }
}
