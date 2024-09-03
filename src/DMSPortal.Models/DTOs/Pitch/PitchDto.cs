using DMSPortal.Models.DTOs.Branch;
using DMSPortal.Models.DTOs.Class;
using DMSPortal.Models.Enums;

namespace DMSPortal.Models.DTOs.Pitch;

public class PitchDto
{
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public string BranchId { get; set; }

    public EPitchStatus Status { get; set; }
    
    public int? NumberOfClasses { get; set; } = 0;
    
    public BranchDto Branch { get; set; }
    
    public IEnumerable<ClassDto> Classes { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public DateTime? DeletedAt { get; set; }
}