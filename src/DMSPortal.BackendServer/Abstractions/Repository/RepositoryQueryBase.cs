using System.Linq.Expressions;
using DMSPortal.BackendServer.Abstractions.Entity;
using DMSPortal.BackendServer.Data;
using Microsoft.EntityFrameworkCore;

namespace DMSPortal.BackendServer.Abstractions.Repository;

public abstract class RepositoryQueryBase<T> : IRepositoryQueryBase<T>
    where T : EntityBase
{
    private readonly ApplicationDbContext _dbContext;

    public RepositoryQueryBase(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<T> FindAll(bool trackChanges = false) =>
        !trackChanges
            ? _dbContext.Set<T>().AsNoTracking().Where(e => !e.IsDeleted)
            : _dbContext.Set<T>().Where(e => !e.IsDeleted);

    public IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
    {
        var items = FindAll(trackChanges);
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false) =>
        !trackChanges
            ? _dbContext.Set<T>().Where(e => !e.IsDeleted).Where(expression).AsNoTracking()
            : _dbContext.Set<T>().Where(e => !e.IsDeleted).Where(expression);

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false,
        params Expression<Func<T, object>>[] includeProperties)
    {
        var items = FindByCondition(expression, trackChanges);
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    public async Task<bool> ExistAsync(Expression<Func<T, bool>> expression) =>
        await _dbContext.Set<T>().Where(e => !e.IsDeleted).AnyAsync(expression);

}

public class RepositoryQueryBase<T, K> : RepositoryQueryBase<T>, IRepositoryQueryBase<T, K>
    where T : IdentityEntityBase<K>
{
    private readonly ApplicationDbContext _dbContext;

    public RepositoryQueryBase(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<T?> GetByIdAsync(string id) =>
        await FindByCondition(x => x.Id.Equals(id))
            .FirstOrDefaultAsync();

    public async Task<T?> GetByIdAsync(string id, params Expression<Func<T, object>>[] includeProperties) =>
        await FindByCondition(x => x.Id.Equals(id), trackChanges: false, includeProperties)
            .FirstOrDefaultAsync();
}