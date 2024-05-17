using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Rent.ADO.NET.Services.Contracts;
using Rent.BLL.Services;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;
using System.Collections;
using Microsoft.IdentityModel.Tokens;

namespace Rent.MVC.Controllers;

[Route("/Owner/[action]")]
public class OwnerController(IOwnerService ownerService, IConnectedArchitecture connectedArchitecture, IMemoryCache memoryCache) : BaseController
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
    {
        try
        {
            var cacheKey = "owners";

            if (!memoryCache.TryGetValue(cacheKey, out IEnumerable<OwnerToGetDto>? owners))
            {
                var response = await ownerService.GetAllOwnersAsync();

                if (!response.Exceptions.IsNullOrEmpty())
                {
                    return StatusCode(500);
                }

                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(50),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromSeconds(20)
                };

                memoryCache.Set(cacheKey, owners, cacheExpiryOptions);
            }

            return Json(DataSourceLoader.Load(owners, loadOptions));
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
        var model = new OwnerToCreateDto();
        var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
        PopulateModel(model, valuesDict!);

        if (!TryValidateModel(model))
            return BadRequest(GetFullErrorMessage(ModelState));

        try
        {
            var result = await ownerService.CreateOwnerAsync(model);

            if (!result.Exceptions.IsNullOrEmpty())
            {
                return StatusCode(500);
            }

            return Ok();
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
        var response = await ownerService.GetOwnerByIdAsync(key);
        if (response.Body is null)
            return StatusCode(409, "Object not found");

        var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
        PopulateModel(response.Body, valuesDict!);

        if (!TryValidateModel(response))
            return BadRequest(GetFullErrorMessage(ModelState));
        try
        {
            _ = await ownerService.UpdateOwnerAsync(response.Body);

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
    public async Task<IActionResult> Delete(string key)
    {
        var response = await ownerService.GetOwnerByIdAsync(ConvertTo<System.Guid>(key));

        if (response.Body is null)
            return StatusCode(409, "Object not found");

        try
        {
            _ = await ownerService.DeleteOwnerAsync(response.Body.OwnerId);

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
                Text = $"{i.City}, {i.Street}, {i.Street}"
            }).OrderBy(i => i.Text);

            return Json(DataSourceLoader.Load(lookup, loadOptions));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}