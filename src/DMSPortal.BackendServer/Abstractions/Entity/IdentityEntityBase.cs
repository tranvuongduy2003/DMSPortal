using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMSPortal.BackendServer.Abstractions.Entity;

public class IdentityEntityBase<TKey> : EntityBase, IIdentityEntityBase<TKey>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [MaxLength(50)]
    public TKey Id { get; set; }
}