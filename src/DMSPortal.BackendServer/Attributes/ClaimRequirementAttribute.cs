using DMSPortal.BackendServer.Authorization;
using DMSPortal.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DMSPortal.BackendServer.Attributes;

public class ClaimRequirementAttribute : TypeFilterAttribute
{
    public ClaimRequirementAttribute(EFunctionCode functionId, ECommandCode commandId)
        : base(typeof(ClaimRequirementFilter))
    {
        Arguments = new object[] { functionId, commandId };
    }
}