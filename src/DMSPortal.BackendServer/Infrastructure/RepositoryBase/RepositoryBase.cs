using System.Collections;
using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.EntityBases;
using DMSPortal.BackendServer.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DMSPortal.BackendServer.Infrastructure.RepositoryBase;

public class RepositoryBase<T> : RepositoryQueryBase<T>, IRepositoryBase<T> where T : EntityBase
{
    protected readonly ApplicationDbContext _dbContext;

    public RepositoryBase(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public Task<IDbContextTransaction> BeginTransactionAsync() => _dbContext.Database.BeginTransactionAsync();

    public async Task EndTransactionAsync()
    {
        await SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
    }

    public Task RollbackTransactionAsync() => _dbContext.Database.RollbackTransactionAsync();

    public T Create(T entity)
    {
        _dbContext.Set<T>().Add(entity);
        return entity;
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        return entity;
    }

    public IEnumerable<T> CreateList(IEnumerable<T> entities)
    {
        _dbContext.Set<T>().AddRange(entities);
        return entities.ToList();
    }

    public async Task<IEnumerable<T>> CreateListAsync(IEnumerable<T> entities)
    {
        await _dbContext.Set<T>().AddRangeAsync(entities);
        return entities.ToList();
    }

    public T Update(T entity)
    {
        _dbContext.Entry(entity).CurrentValues.SetValues(entity);
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).CurrentValues.SetValues(entity);
        return entity;
    }

    public IEnumerable<T> UpdateList(IEnumerable<T> entities)
    {
        _dbContext.Set<T>().AddRange(entities);
        return entities;
    }

    public async Task<IEnumerable<T>> UpdateListAsync(IEnumerable<T> entities)
    {
        await _dbContext.Set<T>().AddRangeAsync(entities);
        return entities;
    }

    public void Delete(T entity) => _dbContext.Set<T>().Remove(entity);

    public async Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }

    public void DeleteList(IEnumerable<T> entities) => _dbContext.Set<T>().RemoveRange(entities);

    public async Task DeleteListAsync(IEnumerable<T> entities)
    {
        _dbContext.Set<T>().RemoveRange(entities);
    }

    public Task<int> SaveChangesAsync() => _dbContext.SaveChangesAsync();
}

public class RepositoryBase<T, K> : RepositoryQueryBase<T, K>, IRepositoryBase<T, K>
    where T : IdentityEntityBase<K>
{
    protected readonly ApplicationDbContext _dbContext;

    public RepositoryBase(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public Task<IDbContextTransaction> BeginTransactionAsync() => _dbContext.Database.BeginTransactionAsync();

    public async Task EndTransactionAsync()
    {
        await SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
    }

    public Task RollbackTransactionAsync() => _dbContext.Database.RollbackTransactionAsync();

    public T Create(T entity)
    {
        _dbContext.Set<T>().Add(entity);
        return entity;
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        return entity;
    }

    public IEnumerable<T> CreateList(IEnumerable<T> entities)
    {
        _dbContext.Set<T>().AddRange(entities);
        return entities.ToList();
    }

    public async Task<IEnumerable<T>> CreateListAsync(IEnumerable<T> entities)
    {
        await _dbContext.Set<T>().AddRangeAsync(entities);
        return entities.ToList();
    }

    public T Update(T entity)
    {
        T exist = _dbContext.Set<T>().Find(entity.Id);
        if (exist == null || exist.IsDeleted) return entity;
        _dbContext.Entry(exist).CurrentValues.SetValues(entity);
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        T exist = _dbContext.Set<T>().Find(entity.Id);
        if (exist == null || exist.IsDeleted) return entity;
        _dbContext.Entry(exist).CurrentValues.SetValues(entity);
        return entity;
    }

    public IEnumerable<T> UpdateList(IEnumerable<T> entities)
    {
        _dbContext.Set<T>().AddRange(entities);
        return entities;
    }

    public async Task<IEnumerable<T>> UpdateListAsync(IEnumerable<T> entities)
    {
        await _dbContext.Set<T>().AddRangeAsync(entities);

        return entities;
    }

    public void Delete(T entity) => _dbContext.Set<T>().Remove(entity);

    public async Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }

    public void DeleteList(IEnumerable<T> entities) => _dbContext.Set<T>().RemoveRange(entities);

    public async Task DeleteListAsync(IEnumerable<T> entities)
    {
        _dbContext.Set<T>().RemoveRange(entities);
    }

    public Task<int> SaveChangesAsync() => _dbContext.SaveChangesAsync();
}