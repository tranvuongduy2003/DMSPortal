using DMSPortal.Models.DTOs.Branch;
using DMSPortal.Models.Enums;

namespace DMSPortal.Models.DTOs.PitchGroup;

public class PitchGroupDto
{
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public int? NumberOfBranches { get; set; } = 0;
    
    public EPitchGroupStatus Status { get; set; }
    
    public IEnumerable<BranchDto> Branches { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public DateTime? DeletedAt { get; set; }
}