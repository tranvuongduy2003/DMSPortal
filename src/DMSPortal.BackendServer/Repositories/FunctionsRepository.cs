using DMSPortal.BackendServer.Abstractions.Repository;
using DMSPortal.BackendServer.Abstractions.Repository.Contracts;
using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;

namespace DMSPortal.BackendServer.Repositories;

public class FunctionsRepository : RepositoryBase<Function, string>, IFunctionsRepository
{
    public FunctionsRepository(ApplicationDbContext dbContext) : base(
        dbContext)
    {
    }
}