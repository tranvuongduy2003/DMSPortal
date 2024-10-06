using DMSPortal.BackendServer.Abstractions.Repository;
using DMSPortal.BackendServer.Abstractions.Repository.Contracts;
using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;

namespace DMSPortal.BackendServer.Repositories;

public class PitchGroupsRepository : RepositoryBase<PitchGroup, string>, IPitchGroupsRepository
{
    public PitchGroupsRepository(ApplicationDbContext dbContext) : base(
        dbContext)
    {
    }
}