﻿namespace DMSPortal.Models.Requests;

public class UpdatePermissionByRoleRequest
{
    public string RoleId { get; set; }

    public string FunctionId { get; set; }

    public bool Value { get; set; }
}