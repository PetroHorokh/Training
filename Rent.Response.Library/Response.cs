namespace Rent.Response.Library;

public class Response<T>
{
    public T? Body { get; set; }

    public List<Exception> Exceptions { get; set; } = new List<Exception>();

    public DateTime TimeStamp { get; set; } = DateTime.Now;
}