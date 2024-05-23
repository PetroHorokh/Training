using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Rent.Auth.DAL.Context;
using Rent.ResponseAndRequestLibrary;

namespace Rent.Auth.DAL.RepositoryBase;

public class RepositoryBase<T>(AuthRentContext context) : IRepositoryBase<T>
    where T : class
{
    protected readonly AuthRentContext Context = context;

    public async Task<Response<IEnumerable<T>>> GetAllAsync(
        params string[] includes)
    {
        var result = new Response<IEnumerable<T>>();

        try
        {
            if (includes.Length == 0)
            {
                result.Body = await Context.Set<T>()
                    .AsNoTracking()
                    .ToListAsync();
            }
            else
            {
                result.Body = await includes.Aggregate(Context.Set<T>().AsNoTracking(),
                    (current, includeProperty) => current.Include(includeProperty)).ToListAsync();
            }
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(ex);
        }

        return result;
    }

    public async Task<Response<IEnumerable<T>>> GetPartialAsync(
        int skip, int take, params string[] includes)
    {
        var result = new Response<IEnumerable<T>>();

        try
        {
            if (includes.Length == 0)
            {
                result.Body = await Context.Set<T>()
                    .Skip(skip)
                    .Take(take)
                    .AsNoTracking()
                    .ToListAsync();
            }
            else
            {
                result.Body = await includes.Aggregate(Context.Set<T>().Skip(skip).Take(take).AsNoTracking(),
                    (current, includeProperty) => current.Include(includeProperty)).ToListAsync();
            }
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(ex);
        }

        return result;
    }

    public async Task<Response<IEnumerable<T>>> GetByConditionAsync(
        Expression<Func<T, bool>> expression, params string[] includes)
    {
        var result = new Response<IEnumerable<T>>();

        try
        {
            if (includes.Length == 0)
            {
                result.Body = await Context.Set<T>()
                    .Where(expression)
                    .AsNoTracking()
                    .ToListAsync();
            }
            else
            {
                result.Body = await includes.Aggregate(Context.Set<T>().Where(expression).AsNoTracking(),
                    (current, includeProperty) => current.Include(includeProperty)).ToListAsync();
            }
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(ex);
        }

        return result;
    }

    public async Task<Response<T>> GetSingleByConditionAsync(
        Expression<Func<T, bool>> expression, params string[] includes)
    {
        var result = new Response<T>();

        try
        {
            if (includes.Length == 0)
            {
                result.Body = await Context.Set<T>()
                    .Where(expression)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
            }
            else
            {
                result.Body = await includes.Aggregate(Context.Set<T>().Where(expression).AsNoTracking(),
                    (current, includeProperty) => current.Include(includeProperty)).FirstOrDefaultAsync();
            }
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(ex);
        }

        return result;
    }

    public Response<EntityEntry<T>> Add(
        T entity)
    {
        var result = new Response<EntityEntry<T>>();

        try
        {
            result.Body = Context.Set<T>().Add(entity);
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(ex);
        }

        return result;
    }

    public Response<EntityEntry<T>> Update(
        T entity)
    {
        var result = new Response<EntityEntry<T>>();

        try
        {
            result.Body = Context.Set<T>().Update(entity);

        }
        catch (Exception ex)
        {
            result.Exceptions.Add(ex);
        }

        return result;
    }
    public Response<EntityEntry<T>> Delete(
        T entity)
    {
        var result = new Response<EntityEntry<T>>();

        try
        {
            result.Body = Context.Set<T>().Remove(entity);
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(ex);
        }

        return result;
    }
}