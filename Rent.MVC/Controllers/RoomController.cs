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
                var response = await roomService.GetAllRoomsAsync();

                if (response.Error is not null)
                {
                    throw response.Error;
                }

                rooms = response.Collection;

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
            var response = await roomService.CreateRoomAsync(model);

            if (response.Error is not null)
            {
                throw response.Error;
            }

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
        var response1 = await roomService.GetRoomByRoomIdAsync(key);

        var room = response1.Entity;

        if (room is null) return StatusCode(409, "Object not found");

        try
        {
            var response2 = await roomService.DeleteRoomAsync(room.RoomId);

            if(response2.Error is not null)
            {
                throw response2.Error;
            }

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
            var response = await roomService.GetAllRoomTypesAsync();

            if (response.Error is not null)
            {
                throw response.Error;
            }

            var lookup = response.Collection!.Select(i => new
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
            var response = await roomService.GetAllAccommodationsAsync();

            if (response.Error is not null)
            {
                throw response.Error;
            }

            var lookup = response.Collection!.Select(i => new
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