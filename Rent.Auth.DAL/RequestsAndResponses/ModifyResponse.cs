using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Rent.Auth.DAL.RequestsAndResponses;

public class ModifyResponse<T> : BaseResponse where T : class
{
    public EntityEntry<T>? Status { get; set; }
}