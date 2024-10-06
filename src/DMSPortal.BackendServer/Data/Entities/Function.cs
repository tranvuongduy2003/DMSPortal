using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMSPortal.BackendServer.Abstractions.Entity;

namespace DMSPortal.BackendServer.Data.Entities;

[Table("Functions")]
public class Function : IdentityEntityBase<string>
{
    [Required]
    [MaxLength(200)]
    [Column(TypeName = "text")]
    public required string Name { get; set; }

    [Required]
    [MaxLength(200)]
    public required string Url { get; set; }

    [Required]
    public int SortOrder { get; set; }

    [MaxLength(50)]
    public string? ParentId { get; set; }

    [ForeignKey("ParentId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Function Parent { get; set; } = null!;

    public virtual ICollection<CommandInFunction> CommandInFunctions { get; set; } = new List<CommandInFunction>();

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}