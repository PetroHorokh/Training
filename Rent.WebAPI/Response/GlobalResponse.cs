using Rent.DAL.RequestsAndResponses;

namespace Rent.WebAPI.Response;

/// <summary>
/// Global response to unify all responses from actions
/// </summary>
public class GlobalResponse
{
    public BaseResponse? Response { get; set; }
}