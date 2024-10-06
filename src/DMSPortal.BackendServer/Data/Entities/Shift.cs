using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMSPortal.BackendServer.Abstractions.Entity;

namespace DMSPortal.BackendServer.Data.Entities;

[Table("Shifts")]
public class Shift : IdentityEntityBase<string>
{
    [Required]
    [Column(TypeName = "text")]
    public string Name { get; set; }

    [Required]
    public string Date { get; set; } // Format: DD/MM/YYYY

    [Required]
    public string StartTime { get; set; } // Format: HH:mm
 
    [Required]
    public string EndTime { get; set; } // Format: HH:mm

    [Range(0, Double.PositiveInfinity)]
    public int? NumberOfClasses { get; set; } = 0;

    public virtual ICollection<ClassInShift> ClassInShifts { get; set; } = new List<ClassInShift>();
    
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}