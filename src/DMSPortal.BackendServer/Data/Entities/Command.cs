using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMSPortal.BackendServer.Abstractions.Entity;

namespace DMSPortal.BackendServer.Data.Entities;

[Table("Commands")]
public class Command : IdentityEntityBase<string>
{
    [Required]
    [MaxLength(50)]
    [Column(TypeName = "text")]
    public string Name { get; set; }

    public virtual ICollection<CommandInFunction> CommandInFunctions { get; set; } = new List<CommandInFunction>();

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}