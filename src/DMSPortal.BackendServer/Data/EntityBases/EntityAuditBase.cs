using DMSPortal.BackendServer.Data.Interfaces;

namespace DMSPortal.BackendServer.Data.EntityBases;

public class EntityAuditBase<TKey> : EntityBase<TKey>, IDateTracking
{
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}