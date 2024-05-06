namespace Rent.DAL.RequestsAndResponses;

public class GetMultipleResponse<T> : BaseResponse where T : class
{
    public IEnumerable<T>? Collection { get; set; }

    public int? Count { get; set; }
}