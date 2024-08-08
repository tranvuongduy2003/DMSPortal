using DMSPortal.Models.Enums;

namespace DMSPortal.Models.DTOs.User;

public class UserDto
{
    public string Id { get; set; }

    public string UserName { get; set; }
    
    public string? FullName { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public DateTimeOffset? Dob { get; set; }

    public EGender? Gender { get; set; }

    public string? Avatar { get; set; }
    
    public string? Address { get; set; }

    public EUserStatus Status { get; set; }

    public List<string> Roles { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }
}