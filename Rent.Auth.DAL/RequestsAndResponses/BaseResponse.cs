namespace Rent.Auth.DAL.RequestsAndResponses;

public abstract class BaseResponse
{
    public Exception? Error { get; set; }

    public DateTime TimeStamp { get; set; } = DateTime.Now;
}