using DMSPortal.BackendServer.Data.Interfaces;

namespace DMSPortal.BackendServer.Data.EntityBases;

public class EntityBase : IEntityBase
{
    public bool IsDeleted { get; set; } = false;
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }
}