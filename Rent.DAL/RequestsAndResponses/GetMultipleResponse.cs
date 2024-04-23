namespace Rent.DAL.RequestsAndResponses;

public class GetMultipleResponse<T> where T : class
{
    public IEnumerable<T>? Collection { get; set; }

    public Exception? Error { get; set; }

    public DateTime TimeStamp { get; set; } = DateTime.Now;
}