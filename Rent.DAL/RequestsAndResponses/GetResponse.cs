namespace Rent.DAL.RequestsAndResponses;

public class GetResponse
{
    public object? Entity { get; set; }

    public Exception? Error { get; set; }

    public DateTime TimeStamp { get; set; } = DateTime.Now;
}