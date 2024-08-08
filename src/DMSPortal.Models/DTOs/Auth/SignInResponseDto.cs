namespace DMSPortal.Models.DTOs.Auth;

public class SignInResponseDto
{
    public string AccessToken { get; set; }
    
    public string RefreshToken { get; set; }
}