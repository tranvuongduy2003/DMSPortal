using DMSPortal.BackendServer.Abstractions.Repository;
using DMSPortal.BackendServer.Abstractions.Repository.Contracts;
using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;

namespace DMSPortal.BackendServer.Repositories;

public class ClassesRepository : RepositoryBase<Class, string>, IClassesRepository
{
    public ClassesRepository(ApplicationDbContext dbContext) : base(
        dbContext)
    {
    }
}