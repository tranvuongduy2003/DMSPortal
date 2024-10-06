using DMSPortal.BackendServer.Abstractions.Repository;
using DMSPortal.BackendServer.Abstractions.Repository.Contracts;
using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;

namespace DMSPortal.BackendServer.Repositories;

public class CommandsRepository : RepositoryBase<Command, string>, ICommandsRepository
{
    public CommandsRepository(ApplicationDbContext dbContext) : base(
        dbContext)
    {
    }
}