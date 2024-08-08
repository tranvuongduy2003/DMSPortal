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
    
    public PitchDto Pitch { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }
}