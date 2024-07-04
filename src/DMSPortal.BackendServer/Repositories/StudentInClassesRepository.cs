using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Infrastructure.RepositoryBase;
using DMSPortal.BackendServer.Repositories.Contracts;

namespace DMSPortal.BackendServer.Repositories;

public class StudentInClassesRepository : RepositoryBase<StudentInClass>, IStudentInClassesRepository
{
    public StudentInClassesRepository(ApplicationDbContext dbContext) : base(
        dbContext)
    {
    }
}