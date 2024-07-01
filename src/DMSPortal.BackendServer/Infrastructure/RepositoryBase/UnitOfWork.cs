using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Infrastructure.Interfaces;
using DMSPortal.BackendServer.Repositories;
using DMSPortal.BackendServer.Repositories.Contracts;

namespace DMSPortal.BackendServer.Infrastructure.RepositoryBase;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IFunctionsRepository Functions { get; private set; }
    public ICommandsRepository Commands { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Functions = new FunctionsRepository(_context);
        Commands = new CommandsRepository(_context);
    }

    public void Dispose() => _context.Dispose();

    public Task<int> CommitAsync() => _context.SaveChangesAsync();
}