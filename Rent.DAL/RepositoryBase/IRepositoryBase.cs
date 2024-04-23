using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Rent.DAL.RequestsAndResponses;

namespace Rent.DAL.RepositoryBase;

public interface IRepositoryBase<T> where T : class
{
    Task<GetMultipleResponse<T>> GetAllAsync();

    Task<GetMultipleResponse<T>> GetPartialAsync(
        int skip, int take);

    Task<GetMultipleResponse<T>> GetByConditionAsync(
        Expression<Func<T, bool>> expression);

    Task<GetSingleResponse<T>> GetSingleByConditionAsync(
        Expression<Func<T, bool>> expression);

    ModifyResponse<T> Update(T entity);
    ModifyResponse<T> Delete(T entity);
}