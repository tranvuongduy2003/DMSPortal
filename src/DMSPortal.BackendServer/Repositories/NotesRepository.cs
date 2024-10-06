using DMSPortal.BackendServer.Abstractions.Repository;
using DMSPortal.BackendServer.Abstractions.Repository.Contracts;
using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;

namespace DMSPortal.BackendServer.Repositories;

public class NotesRepository : RepositoryBase<Note, string>, INotesRepository
{
    public NotesRepository(ApplicationDbContext dbContext) : base(
        dbContext)
    {
    }
}