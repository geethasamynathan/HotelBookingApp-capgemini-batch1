using HotelBookin_Backend.DTO;
using HotelBookin_Backend.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookin_Backend.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDto)
        {
            var result = await _userService.RegisterUser(userRegisterDto);
            return CreatedAtAction(nameof(GetUserProfile), new { userId = result. UserId}, result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDto)
        {
            var result = await _userService.LoginUser(userLoginDto);
            return Ok(result);
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfile(int userId)
        {
            var result = await _userService.GetUserProfile(userId);
            return Ok(result);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserDTO userProfileDto)
        {
            var result = await _userService.UpdateUserProfile(userProfileDto);
            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
           //
            return NoContent();
        }
    }

}
