using DMSPortal.BackendServer.Abstractions.Repository;
using DMSPortal.BackendServer.Abstractions.Repository.Contracts;
using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;

namespace DMSPortal.BackendServer.Repositories;

public class BranchesRepository : RepositoryBase<Branch, string>, IBranchesRepository
{
    public BranchesRepository(ApplicationDbContext dbContext) : base(
        dbContext)
    {
    }
}