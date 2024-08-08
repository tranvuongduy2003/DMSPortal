using DMSPortal.Models.Enums;

namespace DMSPortal.Models.Requests.Pitch;

public class UpdatePitchRequest
{
    public string Id { get; set; }
    
    public string Name { get; set; }

    public EPitchStatus Status { get; set; }
}