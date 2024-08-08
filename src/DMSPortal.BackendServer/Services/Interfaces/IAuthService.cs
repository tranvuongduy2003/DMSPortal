using DMSPortal.Models.DTOs.Auth;
using DMSPortal.Models.DTOs.User;

namespace DMSPortal.BackendServer.Services.Interfaces;

public interface IAuthService
{
    Task<SignInResponseDto?> SignInAsync(string username, string password);
    
    Task<bool> SignOutAsync();
    
    Task<SignInResponseDto?> RefreshTokenAsync(string accessToken, string refreshToken);
    
    Task<bool> ForgotPasswordAsync(string email, string hostUrl);
    
    Task<UserDto?> GetProfileAsync(string accessToken);
}