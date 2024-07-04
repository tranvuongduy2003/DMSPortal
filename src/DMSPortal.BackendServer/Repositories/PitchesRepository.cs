using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Infrastructure.RepositoryBase;
using DMSPortal.BackendServer.Repositories.Contracts;

namespace DMSPortal.BackendServer.Repositories;

public class PitchesRepository : RepositoryBase<Pitch, string>, IPitchesRepository
{
    public PitchesRepository(ApplicationDbContext dbContext) : base(
        dbContext)
    {
    }
}