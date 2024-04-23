using System.ComponentModel.DataAnnotations;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
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
    [HttpGet("{skip:int}/{take:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<TenantToGetDto>>> GetTenantsPartialAsync(int skip, int take)
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

        var tenants = response.Collection!.ToList();

        if (tenants.Count == 0)
        {
            return new NoContentResult();
        }

        return new OkObjectResult(tenants);
    }

    [HttpGet("{tenantId:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<TenantToGetDto>> GetTenantByIdAsync(Guid tenantId)
    {
        var response = await tenantService.GetTenantByIdAsync(tenantId);

        if (response.Error is not null)
        {
            throw response.Error;
        }

        var tenant = response.Entity;

        if (tenant is null)
        {
            throw new NoEntitiesException("There is no such tenant with given id.");
        }

        return tenant;
    }

    [HttpPost]
    public async Task<IActionResult> PostTenantAsync([FromBody] TenantToCreateDto tenant)
    {
        if (!TryValidateModel(tenant))
        {
            throw new ValidationException("Validate tenant data.");
        }

        var result = await tenantService.CreateTenantAsync(tenant);

        if (result.Error is not null)
        {
            throw new ProcessException(result.Error.Message, result.Error);
        }

        return Created();
    }

    [HttpDelete("{tenantId:guid}")]
    public async Task<IActionResult> DeleteTenantAsync(Guid tenantId)
    {
        var result = await tenantService.DeleteTenantAsync(tenantId);

        if (result.Error is not null)
        {
            throw new ProcessException(result.Error.Message, result.Error);
        }

        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> PutTenantAsync([FromBody] TenantToGetDto tenant)
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
}