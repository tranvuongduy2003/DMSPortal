using DMSPortal.BackendServer.Abstractions.Repository;
using DMSPortal.BackendServer.Abstractions.Repository.Contracts;
using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;

namespace DMSPortal.BackendServer.Repositories;

public class ClassInShiftsRepository : RepositoryBase<ClassInShift>, IClassInShiftsRepository
{
    public ClassInShiftsRepository(ApplicationDbContext dbContext) : base(
        dbContext)
    {
    }
}