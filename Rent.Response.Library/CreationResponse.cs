using Rent.Response.Library;

namespace Rent.DAL.RequestsAndResponses;

public class CreationResponse : BaseResponse
{
    public Guid? CreatedId { get; set; }
}