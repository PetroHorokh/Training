using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;
using System.Collections;

namespace Rent.MVC.Controllers;

public class AssetController(IOwnerService ownerService, IRoomService roomService, IMemoryCache memoryCache) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
    {
        try
        {
            var cacheKey = "assets";

            if (!memoryCache.TryGetValue(cacheKey, out IEnumerable<AssetToGetDto>? assets))
            {
                assets = (await ownerService.GetAllAssetsAsync()).ToList();

                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(50),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromSeconds(20)
                };

                memoryCache.Set(cacheKey, assets, cacheExpiryOptions);
            }

            return Json(DataSourceLoader.Load(assets, loadOptions));
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
        var model = new AssetToCreateDto();
        var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
        PopulateModel(model, valuesDict!);

        if (!TryValidateModel(model))
            return BadRequest(GetFullErrorMessage(ModelState));

        try
        {
            var result = await ownerService.CreateAssetAsync(model);

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
    [HttpDelete]
    [Helpers.Authorize]
    public async Task<IActionResult> Delete(string key)
    {
        var asset = await ownerService.GetAssetByIdAsync(ConvertTo<Guid>(key));

        if (asset is null)
            return StatusCode(409, "Object not found");

        try
        {
            _ = await ownerService.DeleteAssetAsync(asset.AssetId);

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> RoomLookup(DataSourceLoadOptions loadOptions)
    {
        try
        {
            var roomTypes = await roomService.GetAllRoomTypesAsync();
            var lookup = (await roomService.GetAllRoomsAsync())
                .Join(roomTypes,
                    l => l.RoomTypeId,
                    roomType => roomType.RoomTypeId,
                    (l, roomType) => new
                    {
                        RoomId = l.RoomId,
                        Number = l.Number,
                        Area = l.Area,
                        RoomType = roomType.Name
                    })
                .Select(i => new
                {
                    Value = i.RoomId,
                    Text = $"Room number: {i.Number}, area: {i.Area}, room type: {i.RoomType}"
                }).OrderBy(i => i.Text);

            return Json(DataSourceLoader.Load(lookup, loadOptions));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> OwnerLookup(DataSourceLoadOptions loadOptions)
    {
        try
        {
            var lookup = (await ownerService.GetAllOwnersAsync()).Select(i => new
            {
                Value = i.OwnerId,
                Text = i.Name.ToString()
            }).OrderBy(i => i.Text);

            return Json(DataSourceLoader.Load(lookup, loadOptions));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}