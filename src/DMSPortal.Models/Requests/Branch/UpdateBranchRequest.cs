using DMSPortal.Models.Enums;

namespace DMSPortal.Models.Requests.Branch;

public class UpdateBranchRequest
{
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public string Address { get; set; }
    
    public string ManagerId { get; set; }
    
    public EBranchStatus Status { get; set; }
}