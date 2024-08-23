using System.Text.Json.Serialization;
using DMSPortal.Models.DTOs.Note;
using DMSPortal.Models.Enums;

namespace DMSPortal.Models.DTOs.Student;

public class StudentDto
{
    public string Id { get; set; }
    
    public string FullName { get; set; }
    
    public string? PhoneNumber { get; set; }

    public DateTime DOB { get; set; }
    
    public string Address { get; set; }
    
    public EGender? Gender { get; set; }
    
    public double? Height { get; set; } // unit: cm
    
    public double? Weight { get; set; } // unit: kg

    public string? FavouritePosition { get; set; }
    
    public string? FatherFullName { get; set; }
    
    public int? FatherBirthYear { get; set; }
    
    public string? FatherAddress { get; set; }
    
    public string? FatherPhoneNumber { get; set; }
    
    public string? FatherEmail { get; set; }
    
    public string? MotherFullName { get; set; }
    
    public int? MotherBirthYear { get; set; }
    
    public string? MotherAddress { get; set; }
    
    public string? MotherPhoneNumber { get; set; }
    
    public string? MotherEmail { get; set; }
    
    public EStudentStatus Status { get; set; }
    
    public int? NumberOfClasses { get; set; } = 0;
    
    public IEnumerable<NoteDto> Notes { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public DateTime? DeletedAt { get; set; }
}