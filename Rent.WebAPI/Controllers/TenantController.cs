using System.ComponentModel.DataAnnotations;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;
using Rent.DAL.RequestsAndResponses;
using Rent.WebAPI.CustomExceptions;

namespace Rent.WebAPI.Controllers;

[ApiVersion(1.0)]
[ApiController]
[Route("[controller]/[action]")]
public class TenantController(ITenantService tenantService) : Controller
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<TenantToGetDto>>> GetAllTenants()
    {
        var response = await tenantService.GetAllTenantsAsync();

        if (response.Error is not null)
        {
            throw response.Error;
        }

        if (response.Count == 0)
        {
            return new NoContentResult();
        }

        return new OkObjectResult(response.Collection);
    }

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

        if (response.Error is not null)
        {
            throw response.Error;
        }

        if (response.Count == 0)
        {
            return new NoContentResult();
        }

        return new OkObjectResult(response.Collection);
    }

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

        if (response.Error is not null)
        {
            throw response.Error;
        }

        if (response.Count == 0)
        {
            return new NoContentResult();
        }

        return new OkObjectResult(response.Collection);
    }

    [HttpGet("{tenantId:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<TenantToGetDto>> GetTenantById(Guid tenantId)
    {
        var response = await tenantService.GetTenantByIdAsync(tenantId, "Address");

        if (response.Error is not null)
        {
            throw response.Error;
        }

        if (response.Entity is null)
        {
            throw new NoEntitiesException("There is no such tenant with given id.");
        }

        return response.Entity;
    }

    [HttpPost]
    public async Task<IActionResult> PostTenant([FromBody] TenantToCreateDto tenant)
    {
        if (!TryValidateModel(tenant))
        {
            throw new ValidationException("Validate tenant data.");
        }

        var response = await tenantService.CreateTenantAsync(tenant);

        if (response.Error is not null)
        {
            throw new ProcessException(response.Error.Message, response.Error);
        }

        return Created();
    }

    [HttpDelete("{tenantId:guid}")]
    public async Task<IActionResult> DeleteTenant(Guid tenantId)
    {
        var result = await tenantService.DeleteTenantAsync(tenantId);

        if (result.Error is not null)
        {
            throw new ProcessException(result.Error.Message, result.Error);
        }

        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> PutTenant([FromBody] TenantToGetDto tenant)
    {
        if (!TryValidateModel(tenant))
        {
            throw new ValidationException("Validate new tenant data.");
        }

        var result = await tenantService.UpdateTenantAsync(tenant);

        if (result.Error is not null)
        {
            throw new ProcessException(result.Error.Message);
        }
        return NoContent();
    }

    [HttpPatch("{tenantId:guid}")]
    public async Task<IActionResult> PatchTenant(Guid tenantId, [FromBody] JsonPatchDocument<TenantToGetDto> patch)
    {
        var response1 = await tenantService.GetTenantByIdAsync(tenantId);

        if (response1.Error is not null)
        {
            throw new ProcessException(response1.Error.Message, response1.Error);
        }

        var patched = response1.Entity!;
        patch.ApplyTo(patched, ModelState);

        if (!TryValidateModel(patched))
        {
            throw new ValidationException("Validate patch data.");
        }

        var response2 = await tenantService.UpdateTenantAsync(patched);

        if (response2.Error is not null)
        {
            throw new ProcessException(response2.Error.Message, response2.Error);
        }

        return NoContent();
    }
}