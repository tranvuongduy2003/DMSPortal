using DMSPortal.BackendServer.Authorization;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Helpers.HttpResponses;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.DTOs;
using DMSPortal.Models.Models;
using DMSPortal.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using ForgotPasswordRequest = DMSPortal.Models.Requests.ForgotPasswordRequest;

namespace DMSPortal.BackendServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly UserManager<User> _userManager;

    public AuthController(IAuthService authService, UserManager<User> userManager)
    {
        _authService = authService;
        _userManager = userManager;
    }

    [HttpPost("signin")]
    [ProducesResponseType(typeof(SignInResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
    {
        var signInResponse = await _authService.SignInAsync(
            request.Username ?? "",
            request.Password ?? "");

        return signInResponse == null
            ? Unauthorized(new ApiUnauthorizedResponse("Invalid credentials"))
            : Ok(new ApiOkResponse(signInResponse, "Sign in successfully!"));
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> LogOut()
    {
        await _authService.SignOutAsync();

        return Ok(new ApiOkResponse(new object(), "Sign out successfully!"));
    }

    [Authorize]
    [HttpPost("refresh-token")]
    [ServiceFilter(typeof(TokenRequirementFilter))]
    [ProducesResponseType(typeof(SignInResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        if (request.RefreshToken.IsNullOrEmpty())
            return BadRequest("Invalid token");

        var accessToken = Request.Headers[HeaderNames.Authorization]
            .ToString()
            .Replace("Bearer ", "");
        if (accessToken.IsNullOrEmpty())
            return Unauthorized(new ApiUnauthorizedResponse("Unauthorized"));

        var refreshResponse = await _authService.RefreshTokenAsync(accessToken, request.RefreshToken);

        return refreshResponse is null
            ? Unauthorized(new ApiUnauthorizedResponse("invalid_refresh_token"))
            : Ok(new ApiOkResponse(refreshResponse, "Refresh token successfully!"));
    }
    
    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        await _authService.ForgotPasswordAsync(request.Email, request.HostUrl);

        return Ok(new ApiOkResponse("Forgot password successfully!"));
    }
    
    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return NotFound(new ApiNotFoundResponse("User does not exist"));

        var result = await _userManager.ResetPasswordAsync(user, request.ResetCode, request.NewPassword);
        if (!result.Succeeded)
            return BadRequest(new ApiBadRequestResponse(result));

        return Ok(new ApiOkResponse());
    }

    [Authorize]
    [HttpGet("profile")]
    [ServiceFilter(typeof(TokenRequirementFilter))]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserProfile()
    {
        var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        if (accessToken.IsNullOrEmpty())
            return Unauthorized(new ApiUnauthorizedResponse("invalid_token"));

        var userDto = await _authService.GetProfileAsync(accessToken);

        return userDto is null
            ? Unauthorized(new ApiUnauthorizedResponse("Unauthorized"))
            : Ok(new ApiOkResponse(userDto));
    }
}