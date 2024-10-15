using HotelBookin_Backend.DTO;

namespace HotelBookin_Backend.IServices
{
    public interface IUserService
    {
        Task<UserDTO> RegisterUser(UserRegisterDTO userRegisterDto);
        Task<string> LoginUser(UserLoginDTO userLoginDto);
        Task<UserDTO> GetUserProfile(int userId);
        Task<UserDTO> UpdateUserProfile(UserDTO userDto);
        Task LogoutUser(int userId);
    }
}
