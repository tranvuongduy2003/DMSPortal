namespace DMSPortal.BackendServer.Data.Interfaces;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; } 
    
    DateTimeOffset? DeletedAt { get; set; }
}