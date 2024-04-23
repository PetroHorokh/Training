using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Rent.DAL.Context;
using Rent.DAL.RequestsAndResponses;

namespace Rent.DAL.RepositoryBase;

public class RepositoryBase<T>(RentContext context) : IRepositoryBase<T>
    where T : class
{
    protected readonly RentContext Context = context;

    public async Task<GetMultipleResponse<T>> GetAllAsync()
    {
        var result = new GetMultipleResponse<T>();

        try
        {
            result.Collection = await Context.Set<T>().AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            result.Error = ex;
        }

        return result;
    }

    public async Task<GetMultipleResponse<T>> GetPartialAsync(int skip, int take)
    {
        var result = new GetMultipleResponse<T>();

        try
        {
            result.Collection = await Context.Set<T>().Skip(skip).Take(take).AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            result.Error = ex;
        }

        return result;
    }

    public async Task<GetMultipleResponse<T>> GetByConditionAsync(Expression<Func<T, bool>> expression)
    {
        var result = new GetMultipleResponse<T>();

        try
        {
            result.Collection = await Context.Set<T>().Where(expression).AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            result.Error = ex;
        }

        return result;
    }

    public async Task<GetSingleResponse<T>> GetSingleByConditionAsync(Expression<Func<T, bool>> expression)
    {
        var result = new GetSingleResponse<T>();

        try
        {
            result.Entity = await Context.Set<T>().Where(expression).AsNoTracking().FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            result.Error = ex;
        }

        return result;
    }

    public ModifyResponse<T> Update(T entity)
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
    public ModifyResponse<T> Delete(T entity)
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