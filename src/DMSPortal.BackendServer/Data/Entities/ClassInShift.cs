using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMSPortal.BackendServer.Data.EntityBases;
using Microsoft.EntityFrameworkCore;

namespace DMSPortal.BackendServer.Data.Entities;

[Table("ClassInShifts")]
[PrimaryKey("ClassId", "ShiftId")]
public class ClassInShift : EntityBase
{
    [Required]
    [MaxLength(50)]
    public string ClassId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ShiftId { get; set; }

    [ForeignKey("ClassId")]
    public virtual Class Class { get; set; }
    
    [ForeignKey("ShiftId")]
    public virtual Shift Shift { get; set; }
}