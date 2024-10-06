namespace DMSPortal.BackendServer.Abstractions.Entity;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; } 
    
    DateTime? DeletedAt { get; set; }
}