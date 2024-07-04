using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Infrastructure.RepositoryBase;
using DMSPortal.BackendServer.Repositories.Contracts;

namespace DMSPortal.BackendServer.Repositories;

public class CommandInFunctionsRepository : RepositoryBase<CommandInFunction>, ICommandInFunctionsRepository
{
    public CommandInFunctionsRepository(ApplicationDbContext dbContext) : base(
        dbContext)
    {
    }
}