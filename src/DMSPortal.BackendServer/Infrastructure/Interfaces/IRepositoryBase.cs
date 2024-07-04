using DMSPortal.BackendServer.Data.EntityBases;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace DMSPortal.BackendServer.Infrastructure.Interfaces;

public interface IRepositoryQueryBase<T> where T : EntityBase
{
    IQueryable<T> FindAll(bool trackChanges = false);
    IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties);
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties);
}

public interface IRepositoryQueryBase<T, K> : IRepositoryQueryBase<T> where T : IdentityEntityBase<K>
{
    Task<T?> GetByIdAsync(string id);
    Task<T?> GetByIdAsync(string id, params Expression<Func<T, object>>[] includeProperties);
}

public interface IRepositoryBase<T> : IRepositoryQueryBase<T> where T : EntityBase
{
    T Create(T entity);
    Task<T> CreateAsync(T entity);
    IEnumerable<T> CreateList(IEnumerable<T> entities);
    Task<IEnumerable<T>> CreateListAsync(IEnumerable<T> entities);
    T Update(T entity);
    Task<T> UpdateAsync(T entity);
    IEnumerable<T> UpdateList(IEnumerable<T> entities);
    Task<IEnumerable<T>> UpdateListAsync(IEnumerable<T> entities);
    void Delete(T entity);
    Task DeleteAsync(T entity);
    void DeleteList(IEnumerable<T> entities);
    Task DeleteListAsync(IEnumerable<T> entities);
    Task<int> SaveChangesAsync();

    Task<IDbContextTransaction> BeginTransactionAsync();
    Task EndTransactionAsync();
    Task RollbackTransactionAsync();
}

public interface IRepositoryBase<T, K> : IRepositoryBase<T>, IRepositoryQueryBase<T, K> where T : IdentityEntityBase<K>
{
}