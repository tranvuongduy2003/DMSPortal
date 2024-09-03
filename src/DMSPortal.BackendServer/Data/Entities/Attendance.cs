using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMSPortal.BackendServer.Data.EntityBases;
using Microsoft.EntityFrameworkCore;

namespace DMSPortal.BackendServer.Data.Entities;

[Table("Attendances")]
public class Attendance : IdentityEntityBase<string>
{
    [Required]
    [MaxLength(50)]
    public string ClassId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ShiftId { get; set; }

    [Required]
    [MaxLength(50)]
    public string StudentId { get; set; }

    public DateTime CheckinAt { get; set; }

    public DateTime? CheckoutAt { get; set; }

    [ForeignKey("ClassId")]
    public virtual Class Class { get; set; }

    [ForeignKey("ShiftId")]
    public virtual Shift Shift { get; set; }
    
    [ForeignKey("StudentId")]
    public virtual Student Student { get; set; }
}