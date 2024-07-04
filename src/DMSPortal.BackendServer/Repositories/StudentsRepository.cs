using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Infrastructure.RepositoryBase;
using DMSPortal.BackendServer.Repositories.Contracts;

namespace DMSPortal.BackendServer.Repositories;

public class StudentsRepository : RepositoryBase<Student, string>, IStudentsRepository
{
    public StudentsRepository(ApplicationDbContext dbContext) : base(
        dbContext)
    {
    }
}