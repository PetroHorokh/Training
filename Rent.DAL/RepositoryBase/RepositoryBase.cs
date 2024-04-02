using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Rent.DAL.RepositoryBase;

public class RepositoryBase<T>(RentContext context) : IRepositoryBase<T>
    where T : class
{
    protected readonly RentContext Context = context;

    public async Task<IEnumerable<T>> GetAllAsync(
        params Expression<Func<T, object>>[] includes)
    {
        var query = Context
            .Set<T>()
            .AsQueryable();

        return await includes
            .Aggregate(query, (current, next) => current.Include(next))
            .ToListAsync();
    }

    public async Task<IEnumerable<T>> GetByConditionAsync(
        Expression<Func<T, bool>> expression,
        params Expression<Func<T, object>>[] includes)
    {
        var query = Context
            .Set<T>()
            .Where(expression)
            .AsQueryable();

        return await includes
            .Aggregate(query, (current, next) => current.Include(next))
            .ToListAsync();
    }

    public async Task<T?> GetSingleByConditionAsync(
        Expression<Func<T, bool>> expression,
        params Expression<Func<T, object>>[] includes)
    {
        var query = Context
            .Set<T>()
            .Where(expression)
            .AsQueryable();

        return await includes
            .Aggregate(query, (current, next) => current.Include(next))
            .FirstOrDefaultAsync();
    }

    public EntityEntry<T> Update(T entity)
    {
        var result = Context.Set<T>().Update(entity);
        return result;
    }
    public EntityEntry<T> Delete(T entity)
    {
        var result = Context.Set<T>().Remove(entity);
        return result; 
    }
}