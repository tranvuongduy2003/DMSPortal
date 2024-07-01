using DMSPortal.BackendServer.Data.Interfaces;
using DMSPortal.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DMSPortal.BackendServer.Data.Entities;

[Table("Users")]
public class User : IdentityUser, IDateTracking
{
    public User()
    {
    }

    public User(string id, string userName, string fullName, string email, string phoneNumber, DateTime dob)
    {
        Id = id;
        UserName = userName;
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        Dob = dob;
    }

    [MaxLength(50)]
    [Column(TypeName = "nvarchar(50)")]
    public string? FullName { get; set; }

    public DateTime? Dob { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EGender? Gender { get; set; }

    [MaxLength(1000)]
    [Column(TypeName = "nvarchar(1000)")]
    public string? Bio { get; set; }

    [Column(TypeName = "nvarchar(max)")] public string? Avatar { get; set; }

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EUserStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}