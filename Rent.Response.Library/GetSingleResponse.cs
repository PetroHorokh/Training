using Rent.Response.Library;

namespace Rent.DAL.RequestsAndResponses;

public class GetSingleResponse<T> : BaseResponse where T : class
{
    public T? Entity { get; set; }
}