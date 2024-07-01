using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMSPortal.BackendServer.Data.Entities;

[Table("CommandInFunctions")]
[PrimaryKey("CommandId", "FunctionId")]
public class CommandInFunction
{
    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string CommandId { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string FunctionId { get; set; }

    [ForeignKey("CommandId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Command Command { get; set; } = null!;

    [ForeignKey("FunctionId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Function Function { get; set; } = null!;
}