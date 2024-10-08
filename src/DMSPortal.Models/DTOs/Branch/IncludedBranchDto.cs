using DMSPortal.Models.Enums;

namespace DMSPortal.Models.DTOs.Branch;

public class IncludedBranchDto
{
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public string Address { get; set; }
    
    public int? NumberOfPitches { get; set; } = 0;
    
    public EBranchStatus Status { get; set; }
}