using Microsoft.AspNetCore.Http;

namespace Rent.Auth.DAL.RequestsAndResponses;

public class PostImageRequest
{
    public IFormFile Image { get; set; } = null!;

    public Guid UserId { get; set; }

    public string? Url { get; set; }
}