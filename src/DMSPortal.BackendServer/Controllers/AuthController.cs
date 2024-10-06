using DMSPortal.BackendServer.Abstractions.Services;
using DMSPortal.BackendServer.Abstractions.UseCases;
using DMSPortal.BackendServer.Attributes;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.Models.DTOs.Auth;
using DMSPortal.Models.DTOs.User;
using DMSPortal.Models.Enums;
using DMSPortal.Models.HttpResponses;
using DMSPortal.Models.Requests.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using ForgotPasswordRequest = DMSPortal.Models.Requests.Auth.ForgotPasswordRequest;

namespace DMSPortal.BackendServer.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthUseCase _authUseCase;
    private readonly UserManager<User> _userManager;

    public AuthController(IAuthUseCase authUseCase, UserManager<User> userManager)
    {
        _authUseCase = authUseCase;
        _userManager = userManager;
    }

    [HttpPost("signin")]
    [ProducesResponseType(typeof(SignInResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
    {
        var signInResponse = await _authUseCase.SignInAsync(
            request.Username ?? "",
            request.Password ?? "");

        return signInResponse == null
            ? Unauthorized(new ApiUnauthorizedResponse("Thông tin đăng nhập không chính xác"))
            : Ok(new ApiOkResponse(signInResponse, "Đăng nhập thành công!"));
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LogOut()
    {
        await _authUseCase.SignOutAsync();

            return Ok(new ApiOkResponse(new object(), "Đăng xuất thành công!"));
    }

    [HttpPost("refresh-token")]
    [Authorize]
    [TokenRequirement()]
    [ApiValidationFilter]
    [ProducesResponseType(typeof(SignInResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        if (request.RefreshToken.IsNullOrEmpty())
            return BadRequest("invalid_token");

        var accessToken = Request.Headers[HeaderNames.Authorization]
            .ToString()
            .Replace("Bearer ", "");
        if (accessToken.IsNullOrEmpty())
            return Unauthorized(new ApiUnauthorizedResponse("Unauthorized"));

        var refreshResponse = await _authUseCase.RefreshTokenAsync(accessToken, request.RefreshToken);

        return refreshResponse is null
            ? Unauthorized(new ApiUnauthorizedResponse("invalid_refresh_token"))
            : Ok(new ApiOkResponse(refreshResponse, "Làm mới token thành công!"));
    }
    
    [HttpPost("forgot-password")]
    [ApiValidationFilter]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        await _authUseCase.ForgotPasswordAsync(request.Email, request.HostUrl);

        return Ok(new ApiOkResponse("Quên mật khẩu thành công!"));
    }
    
    [HttpPost("reset-password")]
    [ApiValidationFilter]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return NotFound(new ApiNotFoundResponse("Người dùng không tồn tại"));

        var result = await _userManager.ResetPasswordAsync(user, request.ResetCode, request.NewPassword);
        if (!result.Succeeded)
            return BadRequest(new ApiBadRequestResponse(result));

        return Ok(new ApiOkResponse());
    }

    [HttpGet("profile")]
    [ClaimRequirement(EFunctionCode.SYSTEM_USER, ECommandCode.VIEW)]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserProfile()
    {
        var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        if (accessToken.IsNullOrEmpty())
            return Unauthorized(new ApiUnauthorizedResponse("invalid_token"));

        var userDto = await _authUseCase.GetProfileAsync(accessToken);

        return userDto is null
            ? Unauthorized(new ApiUnauthorizedResponse("Unauthorized"))
            : Ok(new ApiOkResponse(userDto));
    }
}