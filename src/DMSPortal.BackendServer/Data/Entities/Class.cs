using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMSPortal.BackendServer.Abstractions.Entity;
using DMSPortal.Models.Enums;

namespace DMSPortal.BackendServer.Data.Entities;

[Table("Classes")]
public class Class : IdentityEntityBase<string>
{
    [Required]
    [MaxLength(100)]
    [Column(TypeName = "text")]
    public string Name { get; set; }

    [Required]
    [MaxLength(50)]
    public string PitchId { get; set; }

    public EClassStatus Status { get; set; }

    [Range(0, Double.PositiveInfinity)]
    public int? NumberOfStudents { get; set; } = 0;

    [ForeignKey("PitchId")]
    public virtual Pitch Pitch { get; set; }

    public virtual ICollection<StudentInClass> StudentInClasses { get; set; } = new List<StudentInClass>();
    
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    
    public virtual ICollection<ClassInShift> ClassInShifts { get; set; } = new List<ClassInShift>();
}