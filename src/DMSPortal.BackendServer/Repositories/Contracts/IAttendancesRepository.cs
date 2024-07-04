using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Infrastructure.Interfaces;

namespace DMSPortal.BackendServer.Repositories.Contracts;

public interface IAttendancesRepository : IRepositoryBase<Attendance, string>
{
}