using System.Linq.Expressions;
using Rent.DAL.RequestsAndResponses;
using Rent.Response.Library;


namespace Rent.Auth.DAL.RepositoryBase;

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

    ModifyResponse<T> Add(T entity);
    ModifyResponse<T> Update(T entity);
    ModifyResponse<T> Delete(T entity);
}