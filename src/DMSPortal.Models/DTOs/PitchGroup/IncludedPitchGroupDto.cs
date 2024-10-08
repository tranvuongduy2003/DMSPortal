using DMSPortal.Models.Enums;

namespace DMSPortal.Models.DTOs.PitchGroup;

public class IncludedPitchGroupDto
{
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public int? NumberOfBranches { get; set; } = 0;
    
    public EPitchGroupStatus Status { get; set; }
}