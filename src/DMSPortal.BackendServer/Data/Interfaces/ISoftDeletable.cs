namespace DMSPortal.BackendServer.Data.Interfaces;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; } 
    
    DateTime? DeletedAt { get; set; }
}