﻿using DMSPortal.Models.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DMSPortal.Models.HttpResponses;

public class ApiForbiddenResponse : ApiResponse
{
    public ApiForbiddenResponse(ModelStateDictionary modelState)
        : base(403)
    {
        if (modelState.IsValid)
        {
            throw new ArgumentException("ModelState must be invalid", nameof(modelState));
        }

        Errors = modelState.SelectMany(x => x.Value.Errors)
            .Select(x => x.ErrorMessage).ToArray();
    }

    public ApiForbiddenResponse(IdentityResult identityResult)
        : base(403)
    {
        Errors = identityResult.Errors
            .Select(x => x.Code + " - " + x.Description).ToArray();
    }

    public ApiForbiddenResponse(string message)
        : base(403, message)
    {
    }
}