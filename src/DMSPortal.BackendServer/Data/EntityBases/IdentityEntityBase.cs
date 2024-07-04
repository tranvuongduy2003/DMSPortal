using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMSPortal.BackendServer.Data.Interfaces;

namespace DMSPortal.BackendServer.Data.EntityBases;

public class IdentityEntityBase<TKey> : EntityBase, IIdentityEntityBase<TKey>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [MaxLength(50)]
    public TKey Id { get; set; }
}