using DMSPortal.Models.Enums;

namespace DMSPortal.Models.DTOs.Pitch;

public class IncludedPitchDto
{
    public string Id { get; set; }
    
    public string Name { get; set; }

    public EPitchStatus Status { get; set; }
    
    public int? NumberOfClasses { get; set; } = 0;
}