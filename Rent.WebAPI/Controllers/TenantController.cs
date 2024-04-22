﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;
using Rent.DAL.RequestsAndResponses;
using Rent.WebAPI.CustomExceptions;

namespace Rent.WebAPI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TenantController(ITenantService tenantService) : Controller
{
    [HttpGet("{skip:int}/{take:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTenantsPartialAsync(int skip, int take)
    {
        if (skip < 0 || take < 0)
        {
            throw new ArgumentException("Invalid parameters");
        }

        var request = new GetRequest
        {
            Skip = skip,
            Take = take
        };

        var tenants = (await tenantService.GetTenantsPartialAsync(request)).ToList();

        if (tenants.Count == 0)
        {
            throw new NoEntitiesException("There are no tenants");
        }

        return Ok(tenants);
    }

    [HttpGet("{tenantId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTenantByIdAsync(Guid tenantId)
    {
        var tenant = await tenantService.GetTenantByIdAsync(tenantId);

        if (tenant is null)
        {
            throw new NoEntitiesException("There are no tenants");
        }

        return Ok(tenant);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> PostTenantAsync([FromBody] TenantToCreateDto tenant)
    {
        if (!TryValidateModel(tenant))
        {
            throw new ValidationException("Validate tenant data");
        }

        var result = await tenantService.CreateTenantAsync(tenant);

        if (result.Error is null)
        {
            return Created();
        }

        throw new ProcessException(result.Error.Message);
    }

    [HttpDelete("{tenantId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteTenantAsync(Guid tenantId)
    {
        var result = await tenantService.DeleteTenantAsync(tenantId);

        if (result.Error is null)
        {
            return NoContent();
        }

        throw new ProcessException(result.Error.Message);
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> PutUpdateAsync([FromBody] TenantToGetDto tenant)
    {
        if (!TryValidateModel(tenant))
        {
            throw new ValidationException("Validate new tenant data");
        }

        var result = await tenantService.UpdateTenantAsync(tenant);

        if (result.Error is null)
        {
            return NoContent();
        }

        throw new ProcessException(result.Error.Message);
    }
}