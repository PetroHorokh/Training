using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Rent.DAL.RequestsAndResponses;

public class ModifyResponse<T> where T : class
{
    public EntityEntry<T>? Status { get; set; }

    public Exception? Error { get; set; }

    public DateTime TimeStamp { get; set; } = DateTime.Now;
}