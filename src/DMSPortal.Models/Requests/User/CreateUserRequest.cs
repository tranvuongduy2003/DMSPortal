using System.Text.Json.Serialization;
using DMSPortal.Models.Enums;

namespace DMSPortal.Models.Requests.User;

public class CreateUserRequest
{
    public string UserName { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string? FullName { get; set; }

    public DateTime? Dob { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EGender? Gender { get; set; }

    public string? Avatar { get; set; }

    public string? Address { get; set; }

    public List<string> Roles { get; set; }
}