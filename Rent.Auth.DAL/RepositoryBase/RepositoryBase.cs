using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Rent.Auth.DAL.Context;
using Rent.Auth.DAL.RequestsAndResponses;

namespace Rent.Auth.DAL.RepositoryBase;

public class RepositoryBase<T>(AuthRentContext context) : IRepositoryBase<T>
    where T : class
{
    protected readonly AuthRentContext Context = context;

    public async Task<GetMultipleResponse<T>> GetAllAsync(
        params string[] includes)
    {
        var result = new GetMultipleResponse<T>();

        try
        {
            if (includes.Length == 0)
            {
                result.Collection = await Context.Set<T>()
                    .AsNoTracking()
                    .ToListAsync();
            }
            else
            {
                result.Collection = await includes.Aggregate(Context.Set<T>().AsNoTracking(),
                    (current, includeProperty) => current.Include(includeProperty)).ToListAsync();
            }
            result.Count = result.Collection.Count();
        }
        catch (Exception ex)
        {
            result.Error = ex;
        }

        return result;
    }

    public async Task<GetMultipleResponse<T>> GetPartialAsync(
        int skip, int take, params string[] includes)
    {
        var result = new GetMultipleResponse<T>();

        try
        {
            if (includes.Length == 0)
            {
                result.Collection = await Context.Set<T>()
                    .Skip(skip)
                    .Take(take)
                    .AsNoTracking()
                    .ToListAsync();
            }
            else
            {
                result.Collection = await includes.Aggregate(Context.Set<T>().Skip(skip).Take(take).AsNoTracking(),
                    (current, includeProperty) => current.Include(includeProperty)).ToListAsync();
            }

            result.Count = result.Collection.Count();
        }
        catch (Exception ex)
        {
            result.Error = ex;
        }

        return result;
    }

    public async Task<GetMultipleResponse<T>> GetByConditionAsync(
        Expression<Func<T, bool>> expression, params string[] includes)
    {
        var result = new GetMultipleResponse<T>();

        try
        {
            if (includes.Length == 0)
            {
                result.Collection = await Context.Set<T>()
                    .Where(expression)
                    .AsNoTracking()
                    .ToListAsync();
            }
            else
            {
                result.Collection = await includes.Aggregate(Context.Set<T>().Where(expression).AsNoTracking(),
                    (current, includeProperty) => current.Include(includeProperty)).ToListAsync();
            }
            result.Count = result.Collection.Count();
        }
        catch (Exception ex)
        {
            result.Error = ex;
        }

        return result;
    }

    public async Task<GetSingleResponse<T>> GetSingleByConditionAsync(
        Expression<Func<T, bool>> expression, params string[] includes)
    {
        var result = new GetSingleResponse<T>();

        try
        {
            if (includes.Length == 0)
            {
                result.Entity = await Context.Set<T>()
                    .Where(expression)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
            }
            else
            {
                result.Entity = await includes.Aggregate(Context.Set<T>().Where(expression).AsNoTracking(),
                    (current, includeProperty) => current.Include(includeProperty)).FirstOrDefaultAsync();
            }
        }
        catch (Exception ex)
        {
            result.Error = ex;
        }

        return result;
    }

    public ModifyResponse<T> Add(
        T entity)
    {
        var result = new ModifyResponse<T>();

        try
        {
            result.Status = Context.Set<T>().Add(entity);
        }
        catch (Exception ex)
        {
            result.Error = ex;
        }

        return result;
    }

    public ModifyResponse<T> Update(
        T entity)
    {
        var result = new ModifyResponse<T>();

        try
        {
            result.Status = Context.Set<T>().Update(entity);

        }
        catch (Exception ex)
        {
            result.Error = ex;
        }

        return result;
    }
    public ModifyResponse<T> Delete(
        T entity)
    {
        var result = new ModifyResponse<T>();

        try
        {
            result.Status = Context.Set<T>().Remove(entity);
        }
        catch (Exception ex)
        {
            result.Error = ex;
        }

        return result;
    }
}