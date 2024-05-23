using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Rent.ResponseAndRequestLibrary;

namespace Rent.DAL.RepositoryBase;

public interface IRepositoryBase<T> where T : class
{
    Task<Response<IEnumerable<T>>> GetAllAsync(
        params string[] includes);

    Task<Response<IEnumerable<T>>> GetPartialAsync(
        int skip, int take, params string[] includes);

    Task<Response<IEnumerable<T>>> GetByConditionAsync(
        Expression<Func<T, bool>> expression, params string[] includes);

    Task<Response<T>> GetSingleByConditionAsync(
        Expression<Func<T, bool>> expression, params string[] includes);

    Response<EntityEntry<T>> Update(T entity);
    Response<EntityEntry<T>> Delete(T entity);
}