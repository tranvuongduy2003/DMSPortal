using DMSPortal.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DMSPortal.BackendServer.Abstractions.Entity;

namespace DMSPortal.BackendServer.Data.Entities;

[Table("Users")]
public class User : IdentityUser, IDateTracking
{
    [MaxLength(50)]
    [Column(TypeName = "text")]
    public string? FullName { get; set; }

    public DateTime? Dob { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EGender? Gender { get; set; }

    [Column(TypeName = "text")] 
    public string? Avatar { get; set; }
    
    [Column(TypeName = "text")] 
    public string? Address { get; set; }

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EUserStatus Status { get; set; }

    [Range(0, Double.PositiveInfinity)]
    public int? NumberOfBranches { get; set; } = 0;

    public DateTime CreatedAt { get; set; }
    

    public DateTime? DeletedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
    
    public virtual ICollection<Branch>? Branches { get; set; } = new List<Branch>();
}