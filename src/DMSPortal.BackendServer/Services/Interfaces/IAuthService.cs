using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.Models.DTOs;
using DMSPortal.Models.Models;
using DMSPortal.Models.Requests;

namespace DMSPortal.BackendServer.Services.Interfaces;

public interface IAuthService
{
    Task<SignInResponse?> SignInAsync(string username, string password);
    
    Task<bool> SignOutAsync();
    
    Task<SignInResponse?> RefreshTokenAsync(string accessToken, string refreshToken);
    
    Task<bool> ForgotPasswordAsync(string email, string hostUrl);
    
    Task<UserDto?> GetProfileAsync(string accessToken);
}