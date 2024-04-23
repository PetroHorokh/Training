using System.Collections;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Rent.ADO.NET.Services;
using Rent.BLL.Services;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;

namespace Rent.MVC.Controllers
{
    public class RentController(ITenantService tenantService, IOwnerService ownerService, IRoomService roomService, IMemoryCache memoryCache) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            try
            {
                var cacheKey = "rents";

                if (!memoryCache.TryGetValue(cacheKey, out IEnumerable<RentToGetDto>? rents))
                {
                    rents = (await tenantService.GetAllRentsAsync()).Collection!.ToList();

                    var cacheExpiryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddSeconds(50),
                        Priority = CacheItemPriority.High,
                        SlidingExpiration = TimeSpan.FromSeconds(20)
                    };

                    memoryCache.Set(cacheKey, rents, cacheExpiryOptions);
                }

                return Json(DataSourceLoader.Load(rents, loadOptions));
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
            var model = new RentToCreateDto();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict!);

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            try
            {
                var result = await tenantService.CreateRentAsync(model);

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

        [HttpGet]
        public async Task<IActionResult> AssetLookup(DataSourceLoadOptions loadOptions)
        {
            try
            {
                var rooms = await roomService.GetAllRoomsAsync();
                var lookup = (await ownerService.GetAllAssetsAsync()).Collection!
                    .Join(rooms.Collection!, 
                        l => l.RoomId, 
                        room => room.RoomId, 
                        (l, room) => new 
                        { 
                            AssetId = l.AssetId,
                            RoomId = l.RoomId,
                            Number = room.Number,
                            Area = room.Area,
                        })
                        .Select(i => new { 
                        Value = i.AssetId,
                        Text = $"Room number: {i.Number}"
                    }).OrderBy(i => i.Text);

                return Json(DataSourceLoader.Load(lookup, loadOptions));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> TenantLookup(DataSourceLoadOptions loadOptions)
        {
            try
            {
                var lookup = (await tenantService.GetAllTenantsAsync()).Collection!
                    .Select(i => new {
                        Value = i.TenantId,
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
}
