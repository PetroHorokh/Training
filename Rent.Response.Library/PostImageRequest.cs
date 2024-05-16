using Microsoft.AspNetCore.Http;

namespace Rent.Response.Library;

public class PostImageRequest
{
    public IFormFile Image { get; set; } = null!;

    public string? Url { get; set; }

    public Guid UserId { get; set; }
}