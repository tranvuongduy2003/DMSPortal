using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Infrastructure.RepositoryBase;
using DMSPortal.BackendServer.Repositories.Contracts;

namespace DMSPortal.BackendServer.Repositories;

public class ShiftsRepository : RepositoryBase<Shift, string>, IShiftsRepository
{
    public ShiftsRepository(ApplicationDbContext dbContext) : base(
        dbContext)
    {
    }
}