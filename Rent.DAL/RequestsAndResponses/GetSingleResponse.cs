namespace Rent.DAL.RequestsAndResponses;

public class GetSingleResponse<T> where T : class
{
    public T? Entity { get; set; }

    public Exception? Error { get; set; }

    public DateTime TimeStamp { get; set; } = DateTime.Now;
}