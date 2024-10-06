using DMSPortal.BackendServer.Abstractions.Repository;
using DMSPortal.BackendServer.Abstractions.Repository.Contracts;
using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;

namespace DMSPortal.BackendServer.Repositories;

public class PitchesRepository : RepositoryBase<Pitch, string>, IPitchesRepository
{
    public PitchesRepository(ApplicationDbContext dbContext) : base(
        dbContext)
    {
    }
}