namespace DMSPortal.BackendServer.Data.Interfaces;

public interface IDateTracking
{
    DateTimeOffset CreatedAt { get; set; }

    DateTimeOffset? UpdatedAt { get; set; }
}