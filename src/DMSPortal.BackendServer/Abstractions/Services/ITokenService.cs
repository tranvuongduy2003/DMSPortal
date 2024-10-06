using System.Security.Claims;
using DMSPortal.BackendServer.Data.Entities;

namespace DMSPortal.BackendServer.Abstractions.Services;

public interface ITokenService
{
    Task<string> GenerateAccessTokenAsync(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromToken(string token);
    bool ValidateTokenExpired(string token);
}