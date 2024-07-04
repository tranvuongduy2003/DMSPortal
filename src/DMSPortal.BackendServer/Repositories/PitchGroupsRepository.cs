using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Infrastructure.RepositoryBase;
using DMSPortal.BackendServer.Repositories.Contracts;

namespace DMSPortal.BackendServer.Repositories;

public class PitchGroupsRepository : RepositoryBase<PitchGroup, string>, IPitchGroupsRepository
{
    public PitchGroupsRepository(ApplicationDbContext dbContext) : base(
        dbContext)
    {
    }
}