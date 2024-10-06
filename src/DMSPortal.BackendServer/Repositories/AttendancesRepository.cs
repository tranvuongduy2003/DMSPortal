using DMSPortal.BackendServer.Abstractions.Repository;
using DMSPortal.BackendServer.Abstractions.Repository.Contracts;
using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;

namespace DMSPortal.BackendServer.Repositories;

public class AttendancesRepository : RepositoryBase<Attendance, string>, IAttendancesRepository
{
    public AttendancesRepository(ApplicationDbContext dbContext) : base(
        dbContext)
    {
    }
}