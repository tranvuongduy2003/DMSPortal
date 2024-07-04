using System.ComponentModel;

namespace DMSPortal.Models.Requests;

public class SignInRequest
{
    [DefaultValue("admin")] 
    public string? Username { get; set; } = string.Empty;

    [DefaultValue("Admin@123")]
    public string? Password { get; set; } = string.Empty;
}