using DMSPortal.Models.DTOs.Branch;
using DMSPortal.Models.Enums;

namespace DMSPortal.Models.DTOs.PitchGroup;

public class PitchGroupDto
{
    public string Name { get; set; }
    
    public int? NumberOfBranches { get; set; } = 0;
    
    public EPitchGroupStatus Status { get; set; }
    
    public IEnumerable<BranchDto> Branches { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }
}