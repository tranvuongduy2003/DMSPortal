using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DMSPortal.BackendServer.Abstractions.Entity;
using DMSPortal.Models.Enums;

namespace DMSPortal.BackendServer.Data.Entities;

[Table("Students")]
public class Student : IdentityEntityBase<string>
{
    [Required]
    [MaxLength(50)]
    [Column(TypeName = "text")]
    public string FullName { get; set; }

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    public DateTime DOB { get; set; }

    [Column(TypeName = "text")]
    public string Address { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EGender? Gender { get; set; }

    [Range(0.0, 300.0)]
    public double? Height { get; set; } // unit: cm

    [Range(0.0, 300.0)]
    public double? Weight { get; set; } // unit: kg

    public string? FavouritePosition { get; set; }

    [MaxLength(50)]
    [Column(TypeName = "text")]
    public string? FatherFullName { get; set; }

    [MaxLength(4)]
    public int? FatherBirthYear { get; set; }

    [Column(TypeName = "text")]
    public string? FatherAddress { get; set; }

    [MaxLength(20)]
    public string? FatherPhoneNumber { get; set; }

    [MaxLength(50)]
    [EmailAddress]
    public string? FatherEmail { get; set; }
    
    [MaxLength(50)]
    [Column(TypeName = "text")]
    public string? MotherFullName { get; set; }

    [MaxLength(4)]
    public int? MotherBirthYear { get; set; }

    [Column(TypeName = "text")]
    public string? MotherAddress { get; set; }

    [MaxLength(20)]
    public string? MotherPhoneNumber { get; set; }

    [MaxLength(50)]
    [EmailAddress]
    public string? MotherEmail { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EStudentStatus Status { get; set; }

    [Range(0, Double.PositiveInfinity)]
    public int? NumberOfClasses { get; set; } = 0;
    
    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
    
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    
    public virtual ICollection<StudentInClass> StudentInClasses { get; set; } = new List<StudentInClass>();
}