using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Infrastructure.RepositoryBase;
using DMSPortal.BackendServer.Repositories.Contracts;

namespace DMSPortal.BackendServer.Repositories;

public class CommandsRepository : RepositoryBase<Command, string>, ICommandsRepository
{
    public CommandsRepository(ApplicationDbContext dbContext) : base(
        dbContext)
    {
    }
}