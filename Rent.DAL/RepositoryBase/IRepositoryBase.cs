using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Rent.DAL.RequestsAndResponses;

namespace Rent.DAL.RepositoryBase;

public interface IRepositoryBase<T> where T : class
{
    Task<GetMultipleResponse<T>> GetAllAsync(
        params string[] includes);

    Task<GetMultipleResponse<T>> GetPartialAsync(
        int skip, int take, params string[] includes);

    Task<GetMultipleResponse<T>> GetByConditionAsync(
        Expression<Func<T, bool>> expression, params string[] includes);

    Task<GetSingleResponse<T>> GetSingleByConditionAsync(
        Expression<Func<T, bool>> expression, params string[] includes);

    ModifyResponse<T> Update(T entity);
    ModifyResponse<T> Delete(T entity);
}