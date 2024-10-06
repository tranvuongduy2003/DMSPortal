using DMSPortal.BackendServer.Abstractions.Repository;
using DMSPortal.BackendServer.Abstractions.Repository.Contracts;
using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;

namespace DMSPortal.BackendServer.Repositories;

public class ShiftsRepository : RepositoryBase<Shift, string>, IShiftsRepository
{
    public ShiftsRepository(ApplicationDbContext dbContext) : base(
        dbContext)
    {
    }
}