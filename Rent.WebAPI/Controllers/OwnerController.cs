using System.ComponentModel.DataAnnotations;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;
using Rent.DAL.RequestsAndResponses;
using Rent.WebAPI.CustomExceptions;

namespace Rent.WebAPI.Controllers;

/// <summary>
/// Controller <c>OwnerController</c> for handling owner logic
/// </summary>
/// <param name="ownerService">Service to work with owners</param>
[ApiVersion(1.0)]
[ApiController]
[Route("[controller]/[action]")]
public class OwnerController(IOwnerService ownerService) : Controller
{
    /// <summary>
    /// Gets partial data for owners
    /// </summary>
    /// <param name="skip">Parameter to skip specified amount of owners</param>
    /// <param name="take">Parameter to take specified amount of owners</param>
    /// <returns>Returns list of <see cref="OwnerToGetDto"/> tenants</returns>
    /// <exception cref="ArgumentException">Thrown when parameters are insufficient</exception>
    /// <exception cref="ProcessException">Thrown when an error occured while working with services</exception>
    [HttpGet("{skip:int}/{take:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<OwnerToGetDto>>> GetOwnersPartialAsync(int skip, int take)
    {
        if (skip < 0 || take < 0)
        {
            throw new ArgumentException("Invalid parameters.");
        }

        var request = new GetPartialRequest
        {
            Skip = skip,
            Take = take
        };

        var response = await ownerService.GetOwnersPartialAsync(request);

        if (response.Error is not null)
        {
            throw response.Error;
        }

        var owners = response.Collection!.ToList();

        if (owners.Count == 0)
        {
            return new NoContentResult();
        }

        return new OkObjectResult(owners);
    }

    /// <summary>
    /// Gets owner by their id
    /// </summary>
    /// <param name="ownerId">Parameter to find owner by specified id</param>
    /// <returns>Returns <see cref="OwnerToGetDto"/> owner</returns>
    /// <exception cref="NoEntitiesException">Thrown when there is no such owner with provided id</exception>
    /// <exception cref="ProcessException">Thrown when an error occured while working with services</exception>
    [HttpGet("{ownerId:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<OwnerToGetDto>> GetOwnerByIdAsync(Guid ownerId)
    {
        var response = await ownerService.GetOwnerByIdAsync(ownerId);

        if (response.Error is not null)
        {
            throw response.Error;
        }

        var owner = response.Entity;

        if (owner is null)
        {
            throw new NoEntitiesException("There is no such owner with given id.");
        }

        return owner;
    }

    /// <summary>
    /// Posts new owner
    /// </summary>
    /// <param name="owner">Parameter to create new owner</param>
    /// <returns>Returns created status if successful</returns>
    /// <exception cref="ProcessException">Thrown when an error occured while working with services</exception>
    [HttpPost]
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

    /// <summary>
    /// Deletes existing owner
    /// </summary>
    /// <param name="ownerId">Parameter to find owner by specified id</param>
    /// <returns>Returns no content if successful</returns>
    /// <exception cref="ProcessException">Thrown when an error occured while working with services</exception>
    [HttpDelete("{ownerId:guid}")]
    public async Task<IActionResult> DeleteOwnerAsync(Guid ownerId)
    {
        var result = await ownerService.DeleteOwnerAsync(ownerId);

        if (result.Error is not null)
        {
            throw new ProcessException(result.Error.Message, result.Error);
        }

        return NoContent();
    }

    /// <summary>
    /// Puts data to existing owner
    /// </summary>
    /// <param name="owner">Parameter to update existing owner</param>
    /// <returns>Returns no content if successful</returns>
    /// <exception cref="ProcessException">Thrown when an error occured while working with services</exception>
    [HttpPut]
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