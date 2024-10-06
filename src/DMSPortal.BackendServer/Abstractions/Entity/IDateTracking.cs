namespace DMSPortal.BackendServer.Abstractions.Entity;

public interface IDateTracking
{
    DateTime CreatedAt { get; set; }

    DateTime? UpdatedAt { get; set; }
}