using DMSPortal.BackendServer.Data.Entities;
using System.Security.Claims;

namespace DMSPortal.BackendServer.Services.Interfaces;

public interface ITokenService
{
    Task<string> GenerateAccessTokenAsync(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromToken(string token);
    bool ValidateTokenExpired(string token);
}