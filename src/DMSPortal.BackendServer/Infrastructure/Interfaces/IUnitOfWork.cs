namespace DMSPortal.BackendServer.Infrastructure.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<int> CommitAsync();
}