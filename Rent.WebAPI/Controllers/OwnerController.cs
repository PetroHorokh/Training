using System.ComponentModel.DataAnnotations;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Rent.BLL.Services.Contracts;
using Rent.DTOs.Library;
using Rent.ExceptionLibrary;
using Rent.ResponseAndRequestLibrary;

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
    /// Gets all data for owners
    /// </summary>
    /// <returns>Returns list of <see cref="OwnerToGetDto"/> owners</returns>
    /// <exception cref="ProcessException">Thrown when an error occured inside services</exception>
    [HttpGet]
    [Authorize]
    [ResponseCache(CacheProfileName = "Default30")]
    public async Task<ActionResult<IEnumerable<OwnerToGetDto>>> GetAllOwners()
    {
        var response = await ownerService.GetAllOwnersAsync();

        if (!response.Exceptions.IsNullOrEmpty())
        {
            return StatusCode(500, response.Exceptions);
        }

        if (!response.Body!.Any())
        {
            return new NoContentResult();
        }

        return new OkObjectResult(response.Body);
    }


    /// <summary>
    /// Gets partial data for owners
    /// </summary>
    /// <param name="skip">Parameter to skip specified amount of owners</param>
    /// <param name="take">Parameter to take specified amount of owners</param>
    /// <returns>Returns list of <see cref="OwnerToGetDto"/> tenants</returns>
    /// <exception cref="ArgumentException">Thrown when parameters are insufficient</exception>
    /// <exception cref="ProcessException">Thrown when an error occured inside services</exception>

    [HttpGet("{skip:int}/{take:int}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<OwnerToGetDto>>> GetPartialOwners(int skip, int take)
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

        if (!response.Exceptions.IsNullOrEmpty())
        {
            return StatusCode(500, response.Exceptions);
        }

        if (!response.Body!.Any())
        {
            return new NoContentResult();
        }

        return new OkObjectResult(response.Body);
    }

    /// <summary>
    /// Gets owner by their id
    /// </summary>
    /// <param name="ownerId">Parameter to find owner by specified id</param>
    /// <returns>Returns <see cref="OwnerToGetDto"/> owner</returns>
    /// <exception cref="NoEntitiesException">Thrown when there is no such owner with provided id</exception>
    /// <exception cref="ProcessException">Thrown when an error occured inside services</exception>
    [HttpGet("{ownerId:guid}")]
    [Authorize]
    public async Task<ActionResult<OwnerToGetDto>> GetOwnerById(Guid ownerId)
    {
        var response = await ownerService.GetOwnerByIdAsync(ownerId);

        if (!response.Exceptions.IsNullOrEmpty())
        {
            return StatusCode(500, response.Exceptions);
        }

        var owner = response.Body;

        return owner ?? throw new NoEntitiesException("There is no such owner with given id.");
    }

    /// <summary>
    /// Posts new owner
    /// </summary>
    /// <param name="owner">Parameter to create new owner</param>
    /// <returns>Returns created status if successful</returns>
    /// <exception cref="ProcessException">Thrown when an error occured inside services</exception>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> PostOwner([FromBody] OwnerToCreateDto owner)
    {
        if (!TryValidateModel(owner))
        {
            throw new ValidationException("Validate owner data.");
        }

        var response = await ownerService.CreateOwnerAsync(owner);

        if (!response.Exceptions.IsNullOrEmpty())
        {
            return StatusCode(500, response.Exceptions);
        }

        return Created();
    }

    /// <summary>
    /// Deletes existing owner
    /// </summary>
    /// <param name="ownerId">Parameter to find owner by specified id</param>
    /// <returns>Returns no content if successful</returns>
    /// <exception cref="ProcessException">Thrown when an error occured inside services</exception>
    [HttpDelete("{ownerId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteOwner(Guid ownerId)
    {
        var response = await ownerService.DeleteOwnerAsync(ownerId);

        if (!response.Exceptions.IsNullOrEmpty())
        {
            return StatusCode(500, response.Exceptions);
        }

        return NoContent();
    }

    /// <summary>
    /// Puts data to existing owner
    /// </summary>
    /// <param name="owner">Parameter to update existing owner</param>
    /// <returns>Returns no content if successful</returns>
    /// <exception cref="ProcessException">Thrown when an error occured inside services</exception>
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> PutOwner([FromBody] OwnerToGetDto owner)
    {
        if (!TryValidateModel(owner))
        {
            throw new ValidationException("Validate new owner data.");
        }

        var response = await ownerService.UpdateOwnerAsync(owner);

        if (!response.Exceptions.IsNullOrEmpty())
        {
            return StatusCode(500, response.Exceptions);
        }

        return NoContent();
    }

    /// <summary>
    /// Patch data to existing owner
    /// </summary>
    /// <param name="ownerId">Parameter to find owner by specified id</param>
    /// <param name="patch">Parameter to patch new data to existing owner</param>
    /// <returns>Returns no content if successful</returns>
    /// <exception cref="ProcessException">Thrown when an error occured inside services</exception>
    /// <exception cref="ValidationException">Thrown when patched owner has invalid data</exception>
    [HttpPatch("{ownerId:guid}")]
    [Authorize]
    public async Task<IActionResult> PatchOwner(Guid ownerId, [FromBody] JsonPatchDocument<OwnerToGetDto> patch)
    {
        var response1 = await ownerService.GetOwnerByIdAsync(ownerId);

        if (!response1.Exceptions.IsNullOrEmpty())
        {
            return StatusCode(500, response1.Exceptions);
        }

        var patched = response1.Body!;
        patch.ApplyTo(patched);

        if (!TryValidateModel(patched))
        {
            throw new ValidationException("Validate patch data.");
        }

        var response2 = await ownerService.UpdateOwnerAsync(patched);

        if (!response2.Exceptions.IsNullOrEmpty())
        {
            return StatusCode(500, response2.Exceptions);
        }

        return NoContent();
    }
}