using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Rent.ADO.NET.Services.Contracts;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.Responses;
using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Rent.MVC.Helpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace Rent.MVC.Controllers;

[Route("/Tenant/[action]")]
public class TenantController(
    ITenantService tenantService,
    IConnectedArchitecture connectedArchitecture,
    IMemoryCache memoryCache) : BaseController
{
    public IActionResult Index()
    {
        return new ViewResult
        {
            ViewName = "Index",
        };
    }

    [HttpGet]
    public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
    {
        try
        {
            var cacheKey = "tenants";

            if (!memoryCache.TryGetValue(cacheKey, out IEnumerable<TenantToGetDto>? tenants))
            {
                tenants = (await tenantService.GetAllTenantsAsync()).ToList();

                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(50),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromSeconds(20)
                };

                memoryCache.Set(cacheKey, tenants, cacheExpiryOptions);
            }

            return Json(DataSourceLoader.Load(tenants, loadOptions));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Helpers.Authorize]
    public async Task<IActionResult> Post(string values)
    {
        var model = new TenantToCreateDto();
        var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
        PopulateModel(model, valuesDict!);

        if (!TryValidateModel(model))
            return BadRequest(GetFullErrorMessage(ModelState));

        try
        {
            var result = await tenantService.CreateTenantAsync(model);

            if (result.Error is null)
            {
                return Ok();
            }
            else
            {
                return StatusCode(409, result.Error.Message);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPut]
    [Helpers.Authorize]
    public async Task<IActionResult> Put(Guid key, string values)
    {
        var model = await tenantService.GetTenantByIdAsync(key);
        if (model is null)
            return StatusCode(409, "Object not found");

        var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
        PopulateModel(model, valuesDict!);

        if (!TryValidateModel(model))
            return BadRequest(GetFullErrorMessage(ModelState));

        try
        {
            _ = await tenantService.UpdateTenantAsync(model);

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpDelete]
    [Helpers.Authorize]
    public async Task<IActionResult> Delete(Guid key)
    {
        var tenant = await tenantService.GetTenantByIdAsync(key);

        if (tenant is null)
            return StatusCode(409, "Object not found");

        try
        {
            _ = await tenantService.DeleteTenantAsync(tenant.TenantId);

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> AddressesLookup(DataSourceLoadOptions loadOptions)
    {
        try
        {
            var lookup = (await connectedArchitecture.GetAllAddressesAsync()).Select(i => new
            {
                Value = i.AddressId,
                Text = i.City + ", " + i.Street + ", " + i.Building
            }).OrderBy(i => i.Text);

            return Json(DataSourceLoader.Load(lookup, loadOptions));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}