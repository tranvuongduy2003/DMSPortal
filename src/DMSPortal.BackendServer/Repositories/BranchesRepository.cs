using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Infrastructure.RepositoryBase;
using DMSPortal.BackendServer.Repositories.Contracts;

namespace DMSPortal.BackendServer.Repositories;

public class BranchesRepository : RepositoryBase<Branch, string>, IBranchesRepository
{
    public BranchesRepository(ApplicationDbContext dbContext) : base(
        dbContext)
    {
    }
}