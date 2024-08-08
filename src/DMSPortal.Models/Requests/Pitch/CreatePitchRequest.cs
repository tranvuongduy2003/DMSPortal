using DMSPortal.Models.Enums;

namespace DMSPortal.Models.Requests.Pitch;

public class CreatePitchRequest
{
    public string Name { get; set; }
    
    public string BranchId { get; set; }

    public EPitchStatus Status { get; set; }
}