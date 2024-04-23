using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;
using Rent.DAL.RequestsAndResponses;
using Rent.WebAPI.CustomExceptions;

namespace Rent.WebAPI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class OwnerController(IOwnerService ownerService) : Controller
{
    [HttpGet("{skip:int}/{take:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<OwnerToGetDto>>> GetOwnerPartialAsync(int skip, int take)
    {
        if (skip < 0 || take < 0)
        {
            throw new ArgumentException("Invalid parameters.");
        }

        var request = new GetRequest
        {
            Skip = skip,
            Take = take
        };

        var owners = (await ownerService.GetOwnersPartialAsync(request)).ToList();

        if (owners.Count == 0)
        {
            return new NoContentResult();
        }

        return new OkObjectResult(owners);
    }

    [HttpGet("{ownerId:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<OwnerToGetDto>> GetOwnerByIdAsync(Guid ownerId)
    {
        var owner = await ownerService.GetOwnerByIdAsync(ownerId);

        if (owner is null)
        {
            throw new NoEntitiesException("There is no such owner with given id.");
        }

        return owner;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> PostOwnerAsync([FromBody] OwnerToCreateDto owner)
    {
        if (!TryValidateModel(owner))
        {
            throw new ValidationException("Validate owner data.");
        }

        var result = await ownerService.CreateOwnerAsync(owner);

        if (result.Error is not null)
        {
            throw new ProcessException(result.Error.Message, result.Error);
        }

        return Created();
    }

    [HttpDelete("{ownerId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteOwnerAsync(Guid ownerId)
    {
        var result = await ownerService.DeleteOwnerAsync(ownerId);

        if (result.Error is not null)
        {
            throw new ProcessException(result.Error.Message, result.Error);
        }

        return NoContent();
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> PutOwnerAsync([FromBody] OwnerToGetDto owner)
    {
        if (!TryValidateModel(owner))
        {
            throw new ValidationException("Validate new owner data.");
        }

        var result = await ownerService.UpdateOwnerAsync(owner);

        if (result.Error is not null)
        {
            throw new ProcessException(result.Error.Message, result.Error);
        }

        return NoContent();
    }
}