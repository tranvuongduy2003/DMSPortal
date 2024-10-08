using DMSPortal.Models.Enums;

namespace DMSPortal.Models.DTOs.User;

public class ManagerDto
{
    public string Id { get; set; }
    
    public string? FullName { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public EGender? Gender { get; set; }

    public string? Avatar { get; set; }
}