using DMSPortal.Models.DTOs.Pitch;
using DMSPortal.Models.DTOs.PitchGroup;
using DMSPortal.Models.DTOs.User;
using DMSPortal.Models.Enums;

namespace DMSPortal.Models.DTOs.Branch;

public class BranchDto
{
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public string Address { get; set; }
    
    public int? NumberOfPitches { get; set; } = 0;
    
    public string PitchGroupId { get; set; }
    
    public string ManagerId { get; set; }
    
    public EBranchStatus Status { get; set; }
    
    public PitchGroupDto PitchGroup { get; set; }
    
    public UserDto Manager { get; set; }
    
    public IEnumerable<PitchDto> Pitches { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }
}