using DMSPortal.BackendServer.Helpers.HttpResponses;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.Enums;
using DMSPortal.Models.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace DMSPortal.BackendServer.Authorization;

public class ClaimRequirementFilter : IAuthorizationFilter
{
    private readonly EFunctionCode _functionCode;
    private readonly ECommandCode _commandCode;
    private readonly ITokenService _tokenService;

    public ClaimRequirementFilter(EFunctionCode functionCode, ECommandCode commandCode, ITokenService tokenService)
    {
        _functionCode = functionCode;
        _commandCode = commandCode;
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

        var principal = _tokenService.GetPrincipalFromToken(accessToken);

        var permissionsClaim = principal?.Claims.SingleOrDefault(c => c.Type == SystemConstants.Claims.Permissions);
        if (permissionsClaim != null)
        {
            var permissions = JsonConvert.DeserializeObject<List<string>>(permissionsClaim.Value);
            if (permissions is null || !permissions.Contains(_functionCode + "_" + _commandCode))
                context.Result = new ForbidResult();
        }
        else
        {
            context.Result = new ForbidResult();
        }
    }
}