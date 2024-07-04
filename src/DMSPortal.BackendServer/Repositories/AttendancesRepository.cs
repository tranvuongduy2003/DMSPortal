using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Infrastructure.RepositoryBase;
using DMSPortal.BackendServer.Repositories.Contracts;

namespace DMSPortal.BackendServer.Repositories;

public class AttendancesRepository : RepositoryBase<Attendance, string>, IAttendancesRepository
{
    public AttendancesRepository(ApplicationDbContext dbContext) : base(
        dbContext)
    {
    }
}