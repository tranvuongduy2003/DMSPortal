using DMSPortal.Models.Enums;

namespace DMSPortal.Models.Requests.PitchGroup;

public class CreatePitchGroupRequest
{
    public string Name { get; set; }
    
    public EPitchGroupStatus Status { get; set; }
}