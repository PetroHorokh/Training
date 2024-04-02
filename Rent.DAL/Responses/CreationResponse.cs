namespace Rent.DAL.Responses;

public class CreationResponse
{
    public Guid? CreatedId { get; set; }

    public Exception? Error { get; set; }
}