namespace DMSPortal.Models.Requests.Auth;

public class ForgotPasswordRequest
{
    public required string Email { get; init; }
    
    public required string HostUrl { get; init; }
}