using Microsoft.EntityFrameworkCore.ChangeTracking;
using Rent.DAL.RequestsAndResponses;

namespace Rent.Response.Library;

public class ModifyResponse<T> : BaseResponse where T : class
{
    public EntityEntry<T>? Status { get; set; }
}