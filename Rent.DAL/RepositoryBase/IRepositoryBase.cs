using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Rent.DAL.RepositoryBase;

public interface IRepositoryBase<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(
        params Expression<Func<T, object>>[] includes);
    Task<IEnumerable<T>> GetByConditionAsync(
        Expression<Func<T, bool>> expression,
        params Expression<Func<T, object>>[] includes);

    Task<T?> GetSingleByConditionAsync(
        Expression<Func<T, bool>> expression, 
        params Expression<Func<T, object>>[] includes);

    EntityEntry<T> Update(T entity);
    EntityEntry<T> Delete(T entity);
}