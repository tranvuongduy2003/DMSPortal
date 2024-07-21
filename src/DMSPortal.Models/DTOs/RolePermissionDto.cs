﻿namespace DMSPortal.Models.DTOs;

public class RolePermissionDto
{
    public List<string> FunctionIds { get; set; } = new();
    
    public List<string> FunctionNames { get; set; } = new();
    
    public string RoleId { get; set; }
    
    public string? RoleName { get; set; }
}