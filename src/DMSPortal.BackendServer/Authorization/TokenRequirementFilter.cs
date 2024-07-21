using DMSPortal.BackendServer.Helpers.HttpResponses;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.Enums;
using DMSPortal.Models.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace DMSPortal.BackendServer.Authorization;

public class TokenRequirementFilter : IAuthorizationFilter
{
    private readonly ITokenService _tokenService;

    public TokenRequirementFilter(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var requestAuthorization = context.HttpContext.Request.Headers[HeaderNames.Authorization];
        var responseAuthorization = context.HttpContext.Response.Headers[HeaderNames.Authorization];

        var accessToken = !requestAuthorization.IsNullOrEmpty()
            ? requestAuthorization.ToString().Replace("Bearer ", "")
            : responseAuthorization.ToString().Replace("Bearer ", "");

        if (_tokenService.ValidateTokenExpired(accessToken))
        {
            context.Result = new UnauthorizedObjectResult(new ApiUnauthorizedResponse("invalid_token"));
            return;
        }
    }
}