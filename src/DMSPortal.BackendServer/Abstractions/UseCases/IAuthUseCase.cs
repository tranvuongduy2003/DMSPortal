using DMSPortal.Models.DTOs.Auth;
using DMSPortal.Models.DTOs.User;

namespace DMSPortal.BackendServer.Abstractions.UseCases;

public interface IAuthUseCase
{
    Task<SignInResponseDto?> SignInAsync(string username, string password);
    
    Task<bool> SignOutAsync();
    
    Task<SignInResponseDto?> RefreshTokenAsync(string accessToken, string refreshToken);
    
    Task<bool> ForgotPasswordAsync(string email, string hostUrl);
    
    Task<UserDto?> GetProfileAsync(string accessToken);
}