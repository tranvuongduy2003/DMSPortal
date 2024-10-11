using DMSPortal.Models.Enums;

namespace DMSPortal.Models.Requests.Class;

public class UpdateClassRequest
{
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public string TeacherId { get; set; }

    public EClassStatus Status { get; set; }
}