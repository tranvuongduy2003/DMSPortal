using System.ComponentModel.DataAnnotations;

namespace DMSPortal.Models.Requests.Auth;

public class RefreshTokenRequest
{
    [Required(ErrorMessage = "RefreshToken is required")]
    public required string RefreshToken { get; set; }
}