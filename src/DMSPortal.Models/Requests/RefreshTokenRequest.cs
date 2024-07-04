using System.ComponentModel.DataAnnotations;

namespace DMSPortal.Models.Requests;

public class RefreshTokenRequest
{
    [Required(ErrorMessage = "RefreshToken is required")]
    public required string RefreshToken { get; set; }
}