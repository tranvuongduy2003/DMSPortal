using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Infrastructure.RepositoryBase;
using DMSPortal.BackendServer.Repositories.Contracts;

namespace DMSPortal.BackendServer.Repositories;

public class NotesRepository : RepositoryBase<Note, string>, INotesRepository
{
    public NotesRepository(ApplicationDbContext dbContext) : base(
        dbContext)
    {
    }
}