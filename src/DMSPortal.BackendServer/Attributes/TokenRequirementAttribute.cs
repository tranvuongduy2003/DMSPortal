using DMSPortal.BackendServer.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DMSPortal.BackendServer.Attributes;

public class TokenRequirementAttribute : TypeFilterAttribute
{
    public TokenRequirementAttribute()
        : base(typeof(TokenRequirementFilter))
    {
        Arguments = new object[] { };
    }
}