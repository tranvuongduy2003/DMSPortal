using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Infrastructure.RepositoryBase;
using DMSPortal.BackendServer.Repositories.Contracts;

namespace DMSPortal.BackendServer.Repositories;

public class ClassesRepository : RepositoryBase<Class, string>, IClassesRepository
{
    public ClassesRepository(ApplicationDbContext dbContext) : base(
        dbContext)
    {
    }
}