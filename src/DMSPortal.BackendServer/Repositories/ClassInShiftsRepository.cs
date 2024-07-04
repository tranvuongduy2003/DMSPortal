using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Infrastructure.RepositoryBase;
using DMSPortal.BackendServer.Repositories.Contracts;

namespace DMSPortal.BackendServer.Repositories;

public class ClassInShiftsRepository : RepositoryBase<ClassInShift>, IClassInShiftsRepository
{
    public ClassInShiftsRepository(ApplicationDbContext dbContext) : base(
        dbContext)
    {
    }
}