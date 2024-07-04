namespace DMSPortal.Models.Requests;

public class ForgotPasswordRequest
{
    public required string Email { get; init; }
    
    public required string HostUrl { get; init; }
}