using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Infrastructure.RepositoryBase;
using DMSPortal.BackendServer.Repositories.Contracts;

namespace DMSPortal.BackendServer.Repositories;

public class FunctionsRepository : RepositoryBase<Function, string>, IFunctionsRepository
{
    public FunctionsRepository(ApplicationDbContext dbContext) : base(
        dbContext)
    {
    }
}