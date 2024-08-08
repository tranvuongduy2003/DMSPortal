using Microsoft.AspNetCore.Mvc;

namespace DMSPortal.BackendServer.Authorization;

public class TokenRequirementAttribute : TypeFilterAttribute
{
    public TokenRequirementAttribute()
        : base(typeof(TokenRequirementFilter))
    {
        Arguments = new object[] { };
    }
}