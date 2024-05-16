using System.ComponentModel.DataAnnotations;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;
using Rent.DAL.RequestsAndResponses;
using Rent.ExceptionLibrary;

namespace Rent.WebAPI.Controllers;

/// <summary>
/// Controller <c>TenantController</c> for handling tenant logic
/// </summary>
/// <param name="tenantService">Service to work with tenants</param>
[ApiVersion(1.0)]
[ApiController]
[Route("[controller]/[action]")]
public class TenantController(ITenantService tenantService) : Controller
{
    /// <summary>
    /// Gets all data for tenants
    /// </summary>
    /// <returns>Returns list of <see cref="TenantToGetDto"/> tenants</returns>
    /// <exception cref="ProcessException">Thrown when an error occured inside services</exception>
    [HttpGet]
    [AllowAnonymous]
    [ResponseCache(CacheProfileName = "Default30")]
    public async Task<ActionResult<IEnumerable<TenantToGetDto>>> GetAllTenants()
    {
        var response = await tenantService.GetAllTenantsAsync();

        if (!response.Exceptions.IsNullOrEmpty())
        {
            return StatusCode(500, response.Exceptions);
        }

        if (!response.Body!.Any())
        {
            return new NoContentResult();
        }

        return response.Body!.ToList();
    }

    /// <summary>
    /// Gets partial data for tenants
    /// </summary>
    /// <param name="skip">Parameter to skip specified amount of tenants</param>
    /// <param name="take">Parameter to take specified amount of tenants</param>
    /// <returns>Returns list of <see cref="TenantToGetDto"/> tenants</returns>
    /// <exception cref="ArgumentException">Thrown when parameters are insufficient</exception>
    /// <exception cref="ProcessException">Thrown when an error occured inside services</exception>

    [HttpGet("{skip:int}/{take:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<TenantToGetDto>>> GetPartialTenants(int skip, int take)
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

        var response = await tenantService.GetTenantsPartialAsync(request);

        if (!response.Exceptions.IsNullOrEmpty())
        {
            return StatusCode(500, response.Exceptions);
        }

        if (!response.Body!.Any())
        {
            return new NoContentResult();
        }

        return response.Body!.ToList();
    }

    /// <summary>
    /// Gets filtered data for tenants
    /// </summary>
    /// <param name="filterString">Parameter to filter data by</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Thrown when parameter is insufficient</exception>
    /// <exception cref="ProcessException">Thrown when an error occured inside services</exception>
    [HttpGet("{filterString:required}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<TenantToGetDto>>> GetFilteredTenants(string filterString)
    {
        if (filterString is null || filterString.Length == 0)
        {
            throw new ArgumentException("Invalid parameters.");
        }

        var request = new GetFilteredRequest
        {
            Filter = filterString
        };

        var response = await tenantService.GetFilterTenantsAsync(request);

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
    /// Gets tenant by their id
    /// </summary>
    /// <param name="tenantId">Parameter to find tenant by specified id</param>
    /// <returns>Returns <see cref="TenantToGetDto"/> tenant</returns>
    /// <exception cref="NoEntitiesException">Thrown when there is no such tenant with provided id</exception>
    /// <exception cref="ProcessException">Thrown when an error occured inside services</exception>
    [HttpGet("{tenantId:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<TenantToGetDto>> GetTenantById(Guid tenantId)
    {
        var response = await tenantService.GetTenantByIdAsync(tenantId, "Address");

        if (!response.Exceptions.IsNullOrEmpty())
        {
            return StatusCode(500, response.Exceptions);
        }

        if (response.Body is null)
        {
            throw new NoEntitiesException("There is no such tenant with given id.");
        }

        return response.Body;
    }

    /// <summary>
    /// Posts new tenant
    /// </summary>
    /// <param name="tenant">Parameter to create new tenant</param>
    /// <returns>Returns created status if successful</returns>
    /// <exception cref="ProcessException">Thrown when an error occured inside services</exception>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> PostTenant([FromBody] TenantToCreateDto tenant)
    {
        var response = await tenantService.CreateTenantAsync(tenant);

        if (!response.Exceptions.IsNullOrEmpty())
        {
            return StatusCode(500, response.Exceptions);
        }

        return Created();
    }

    /// <summary>
    /// Deletes existing tenant
    /// </summary>
    /// <param name="tenantId">Parameter to find tenant by specified id</param>
    /// <returns>Returns no content if successful</returns>
    /// <exception cref="ProcessException">Thrown when an error occured inside services</exception>
    [HttpDelete("{tenantId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteTenant(Guid tenantId)
    {
        var response = await tenantService.DeleteTenantAsync(tenantId);

        if (!response.Exceptions.IsNullOrEmpty())
        {
            return StatusCode(500, response.Exceptions);
        }

        return NoContent();
    }

    /// <summary>
    /// Puts data to existing tenant
    /// </summary>
    /// <param name="tenant">Parameter to update existing tenant</param>
    /// <returns>Returns no content if successful</returns>
    /// <exception cref="ProcessException">Thrown when an error occured inside services</exception>
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> PutTenant([FromBody] TenantToGetDto tenant)
    {
        var response = await tenantService.UpdateTenantAsync(tenant);

        if (!response.Exceptions.IsNullOrEmpty())
        {
            return StatusCode(500, response.Exceptions);
        }

        return NoContent();
    }

    /// <summary>
    /// Patch data to existing tenant
    /// </summary>
    /// <param name="tenantId">Parameter to find tenant by specified id</param>
    /// <param name="patch">Parameter to patch new data to existing tenant</param>
    /// <returns>Returns no content if successful</returns>
    /// <exception cref="ProcessException">Thrown when an error occured inside services</exception>
    /// <exception cref="ValidationException">Thrown when patched tenant has invalid data</exception>
    [HttpPatch("{tenantId:guid}")]
    [Authorize]
    public async Task<IActionResult> PatchTenant(Guid tenantId, [FromBody] JsonPatchDocument<TenantToGetDto> patch)
    {
        var response1 = await tenantService.GetTenantByIdAsync(tenantId);

        if (!response1.Exceptions.IsNullOrEmpty())
        {
            return StatusCode(500, response1.Exceptions);
        }

        var patched = response1.Body!;
        patch.ApplyTo(patched, ModelState);

        if (!TryValidateModel(patched))
        {
            throw new ValidationException("Validate patch data.");
        }

        var response2 = await tenantService.UpdateTenantAsync(patched);

        if (!response2.Exceptions.IsNullOrEmpty())
        {
            return StatusCode(500, response2.Exceptions);
        }

        return NoContent();
    }
}