using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMSPortal.BackendServer.Abstractions.Entity;

namespace DMSPortal.BackendServer.Data.Entities;

[Table("Permissions")]
[PrimaryKey("FunctionId", "RoleId", "CommandId")]
public class Permission : EntityBase
{
    public Permission(string functionId, string roleId, string commandId)
    {
        FunctionId = functionId;
        RoleId = roleId;
        CommandId = commandId;
    }

    [Required]
    [MaxLength(50)]
    public string FunctionId { get; set; }

    [Required]
    [MaxLength(50)]
    public string RoleId { get; set; }

    [Required]
    [MaxLength(50)]
    public string CommandId { get; set; }

    [ForeignKey("FunctionId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Function Function { get; set; } = null!;

    [ForeignKey("RoleId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Role Role { get; set; } = null!;

    [ForeignKey("CommandId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Command Command { get; set; } = null!;
}