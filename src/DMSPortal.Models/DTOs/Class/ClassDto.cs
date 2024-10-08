using DMSPortal.Models.DTOs.Pitch;
using DMSPortal.Models.Enums;

namespace DMSPortal.Models.DTOs.Class;

public class ClassDto
{
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public string PitchId { get; set; }

    public EClassStatus Status { get; set; }
    
    public int? NumberOfStudents { get; set; } = 0;
    
    public IncludedPitchDto Pitch { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public DateTime? DeletedAt { get; set; }
}