﻿using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.Models.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DMSPortal.BackendServer.Abstractions.Services;
using DMSPortal.Models.Constants;

namespace DMSPortal.BackendServer.Services;

public class TokenService : ITokenService
{
    private readonly JwtOptions _jwtOptions;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public TokenService(ApplicationDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager,
        JwtOptions jwtOptions)
    {
        _jwtOptions = jwtOptions;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<string> GenerateAccessTokenAsync(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

        var roles = await _userManager.GetRolesAsync(user);
        var query = from p in _context.Permissions
                    join c in _context.Commands
                        on p.CommandId equals c.Id
                    join f in _context.Functions
                        on p.FunctionId equals f.Id
                    join r in _roleManager.Roles on p.RoleId equals r.Id
                    where roles.Contains(r.Name)
                    select f.Id + "_" + c.Id;
        var permissions = await query.Distinct().ToListAsync();

        var claimList = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, user.Id),
            new Claim(ClaimTypes.Role, string.Join(";", roles)),
            new Claim(SystemConstants.Claims.Permissions, JsonConvert.SerializeObject(permissions)),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            Subject = new ClaimsIdentity(claimList),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        );

        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("invalid_token");

        return principal;
    }

    public bool ValidateTokenExpired(string token)
    {
        if (token.IsNullOrEmpty()) return true;

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtToken = tokenHandler.ReadToken(token);

        if (jwtToken is null) return true;

        return jwtToken.ValidTo < DateTime.UtcNow;
    }
}