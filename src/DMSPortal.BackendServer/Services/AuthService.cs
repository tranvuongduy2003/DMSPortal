using AutoMapper;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.DTOs;
using DMSPortal.Models.Enums;
using DMSPortal.Models.Models;
using DMSPortal.Models.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace DMSPortal.BackendServer.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;
    private readonly IHangfireService _hangfireService;
    private readonly IHostingEnvironment _environment;

    public AuthService(UserManager<User> userManager,
        SignInManager<User> signInManager, ITokenService tokenService, IEmailService emailService, IMapper mapper,
        IHangfireService hangfireService, IHostingEnvironment environment)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _emailService = emailService;
        _mapper = mapper;
        _hangfireService = hangfireService;
        _environment = environment;
    }

    public async Task<SignInResponse?> SignInAsync(string username, string password)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.UserName == username);

        if (user is null || user.Status == EUserStatus.DISABLED)
            return null;

        var isValid = await _userManager.CheckPasswordAsync(user, password);
        if (isValid == false)
            return null;

        await _signInManager.PasswordSignInAsync(user, password, false, false);

        var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
        var refreshToken =
            await _userManager.GenerateUserTokenAsync(user, TokenProviders.DEFAULT, TokenTypes.REFRESH);

        await _userManager.SetAuthenticationTokenAsync(user, TokenProviders.DEFAULT, TokenTypes.REFRESH,
            refreshToken);

        return new SignInResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<bool> SignOutAsync()
    {
        await _signInManager.SignOutAsync();
        return true;
    }

    public async Task<SignInResponse?> RefreshTokenAsync(string accessToken, string refreshToken)
    {
        var principal = _tokenService.GetPrincipalFromToken(accessToken);

        var user = await _userManager.FindByIdAsync(
            principal?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value ?? "");
        if (user == null)
            return null;

        var isValid = await _userManager.VerifyUserTokenAsync(user, TokenProviders.DEFAULT, TokenTypes.REFRESH,
            refreshToken);
        if (!isValid)
            return null;

        var newAccessToken = await _tokenService.GenerateAccessTokenAsync(user);
        var newRefreshToken =
            await _userManager.GenerateUserTokenAsync(user, TokenProviders.DEFAULT, TokenTypes.REFRESH);

        return new SignInResponse()
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    public async Task<bool> ForgotPasswordAsync(string email, string hostUrl)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return false;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        // var resetPasswordUrl = $"https://localhost:5173/reset-password?token={token}&email={request.Email}";
        var resetPasswordUrl = $"{hostUrl}?token={token}&email={email}";

        _hangfireService.Enqueue(() => 
            SendResetPasswordEmailAsync(email, resetPasswordUrl).Wait());

        return true;
    }

    public async Task<UserDto?> GetProfileAsync(string accessToken)
    {
        var principal = _tokenService.GetPrincipalFromToken(accessToken);

        var user = await _userManager.FindByIdAsync(
            principal?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value ?? "");
        if (user == null)
            return null;

        // var avatar = await _fileStorage.GetFileByFileIdAsync(user.AvatarId);
        var userDto = _mapper.Map<UserDto>(user);
        var roles = await _userManager.GetRolesAsync(user);
        userDto.Roles = roles.ToList();

        return userDto;
    }

    public async Task SendResetPasswordEmailAsync(string email, string resetPasswordUrl)
    {
        var fullPath = Path.Combine(_environment.WebRootPath, "Templates", "ResetPasswordEmailTemplate.html");

        using var str = new StreamReader(fullPath);

        var mailText = await str.ReadToEndAsync();

        str.Close();

        mailText = mailText.Replace("[resetPasswordUrl]", resetPasswordUrl);

        await _emailService.SendEmailAsync(email, "Reset Your Password", mailText);
    }
}