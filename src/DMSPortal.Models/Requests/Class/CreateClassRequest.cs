using DMSPortal.Models.Enums;

namespace DMSPortal.Models.Requests.Class;

public class CreateClassRequest
{
    public string Name { get; set; }
    
    public string PitchId { get; set; }
    
    public string TeacherId { get; set; }

    public EClassStatus Status { get; set; }
}