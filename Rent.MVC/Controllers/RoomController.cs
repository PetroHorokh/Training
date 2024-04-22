using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Rent.BLL.Services;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using System.Collections;

namespace Rent.MVC.Controllers;

public class RoomController(IRoomService roomService, IMemoryCache memoryCache) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
    {
        try
        {
            var cacheKey = "rooms";

            if (!memoryCache.TryGetValue(cacheKey, out IEnumerable<RoomToGetDto>? rooms))
            {
                rooms = (await roomService.GetAllRoomsAsync()).ToList();

                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(50),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromSeconds(20)
                };

                memoryCache.Set(cacheKey, rooms, cacheExpiryOptions);
            }

            return Json(DataSourceLoader.Load(rooms, loadOptions));
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
        var model = new RoomToCreateDto();
        var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
        PopulateModel(model, valuesDict!);

        if (!TryValidateModel(model))
            return BadRequest(GetFullErrorMessage(ModelState));

        try
        {
            var result = await roomService.CreateRoomAsync(model);

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
    public async Task<IActionResult> Delete(Guid key)
    {
        var room = await roomService.GetRoomByRoomIdAsync(key);

        if (room is null)
            return StatusCode(409, "Object not found");

        try
        {
            _ = await roomService.DeleteRoomAsync(room.RoomId);

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> RoomTypeLookup(DataSourceLoadOptions loadOptions)
    {
        try
        {
            var lookup = (await roomService.GetAllRoomTypesAsync()).Select(i => new
            {
                Value = i.RoomTypeId,
                Text = i.Name
            }).OrderBy(i => i.Text);

            return Json(DataSourceLoader.Load(lookup, loadOptions));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> AccommodationLookup(DataSourceLoadOptions loadOptions)
    {
        try
        {
            var lookup = (await roomService.GetAllAccommodationsAsync()).Select(i => new
            {
                Value = i.AccommodationId,
                Text = i.Name
            }).OrderBy(i => i.Text);

            return Json(DataSourceLoader.Load(lookup, loadOptions));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}