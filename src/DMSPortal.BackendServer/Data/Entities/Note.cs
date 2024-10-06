using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMSPortal.BackendServer.Abstractions.Entity;

namespace DMSPortal.BackendServer.Data.Entities;

[Table("Notes")]
public class Note : IdentityEntityBase<string>
{
    [Required]
    [Column(TypeName = "text")]
    public string Content { get; set; }

    [Required]
    [MaxLength(50)]
    public string StudentId { get; set; }

    [ForeignKey("StudentId")]
    public virtual Student Student { get; set; }
}