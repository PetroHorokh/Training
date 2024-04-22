using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using System.Collections;
using System.Globalization;
using Rent.DAL.Authentication;
using static System.String;

namespace Rent.MVC.Controllers;

public class BaseController : Controller
{
    protected T ConvertTo<T>(object value)
    {
        var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
        return ((T)converter.ConvertFrom(null, CultureInfo.InvariantCulture, value)!)!;
    }

    protected string GetFullErrorMessage(ModelStateDictionary modelState)
    {
        var messages = new List<string>();

        foreach (var entry in modelState)
        {
            foreach (var error in entry.Value.Errors)
                messages.Add(error.ErrorMessage);
        }

        return Join(" ", messages);
    }

    protected void PopulateModel(RoomToCreateDto model, IDictionary values)
    {
        string number = nameof(RoomToCreateDto.Number);
        string area = nameof(RoomToCreateDto.Area);
        string roomTypeId = nameof(RoomToCreateDto.RoomTypeId);

        if (values.Contains(number))
        {
            model.Number = Convert.ToInt32(values[number]);
        }

        if (values.Contains(area))
        {
            model.Area = Convert.ToDecimal(values[area]);
        }

        if (values.Contains(roomTypeId))
        {
            model.RoomTypeId = Convert.ToInt32(values[roomTypeId]);
        }
    }

    protected void PopulateModel(AssetToCreateDto model, IDictionary values)
    {
        string ownerId = nameof(AssetToCreateDto.OwnerId);
        string roomId = nameof(AssetToCreateDto.RoomId);

        if (values.Contains(ownerId))
        {
            model.OwnerId = ConvertTo<Guid>(values[ownerId]!);
        }

        if (values.Contains(roomId))
        {
            model.RoomId = ConvertTo<Guid>(values[roomId]!);
        }
    }

    protected void PopulateModel(OwnerToGetDto model, IDictionary values)
    {
        string ownerId = nameof(OwnerToGetDto.OwnerId);
        string name = nameof(OwnerToGetDto.Name);
        string userId = nameof(OwnerToGetDto.UserId);
        string addressId = nameof(OwnerToGetDto.AddressId);

        if (values.Contains(ownerId))
        {
            model.UserId = ConvertTo<Guid>(values[ownerId]!);
        }

        if (values.Contains(name))
        {
            model.Name = Convert.ToString(values[name])!;
        }

        if (values.Contains(userId))
        {
            model.UserId = ConvertTo<Guid>(values[userId]!);
        }

        if (values.Contains(addressId))
        {
            model.AddressId = ConvertTo<Guid>(values[addressId]!);
        }
    }

    protected void PopulateModel(OwnerToCreateDto model, IDictionary values)
    {
        string name = nameof(OwnerToCreateDto.Name);
        string userId = nameof(OwnerToCreateDto.UserId);
        string addressId = nameof(OwnerToCreateDto.AddressId);

        if (values.Contains(name))
        {
            model.Name = Convert.ToString(values[name])!;
        }

        if (values.Contains(userId))
        {
            model.UserId = ConvertTo<Guid>(values[userId]!);
        }

        if (values.Contains(addressId))
        {
            model.AddressId = ConvertTo<Guid>(values[addressId]!);
        }
    }

    protected void PopulateModel(TenantToGetDto model, IDictionary values)
    {
        string tenantId = nameof(TenantToGetDto.TenantId);
        string name = nameof(TenantToGetDto.Name);
        string bankName = nameof(TenantToGetDto.BankName);
        string addressId = nameof(TenantToGetDto.AddressId);
        string director = nameof(TenantToGetDto.Director);
        string description = nameof(TenantToGetDto.Description);

        if (values.Contains(tenantId))
        {
            model.TenantId = ConvertTo<Guid>(values[tenantId]!);
        }

        if (values.Contains(name))
        {
            model.Name = Convert.ToString(values[name])!;
        }

        if (values.Contains(bankName))
        {
            model.BankName = Convert.ToString(values[bankName])!;
        }

        if (values.Contains(addressId))
        {
            model.AddressId = ConvertTo<Guid>(values[addressId]!);
        }

        if (values.Contains(director))
        {
            model.Director = Convert.ToString(values[director])!;
        }

        if (values.Contains(description))
        {
            model.Description = Convert.ToString(values[description])!;
        }
    }

    protected void PopulateModel(TenantToCreateDto model, IDictionary values)
    {
        string name = nameof(TenantToCreateDto.Name);
        string userId = nameof(TenantToCreateDto.UserId);
        string bankName = nameof(TenantToCreateDto.BankName);
        string addressId = nameof(TenantToCreateDto.AddressId);
        string director = nameof(TenantToCreateDto.Director);
        string description = nameof(TenantToCreateDto.Description);

        if (values.Contains(name))
        {
            model.Name = Convert.ToString(values[name])!;
        }

        if (values.Contains(userId))
        {
            model.UserId = ConvertTo<Guid>(values[userId]!);
        }

        if (values.Contains(bankName))
        {
            model.BankName = Convert.ToString(values[bankName])!;
        }

        if (values.Contains(addressId))
        {
            model.AddressId = ConvertTo<Guid>(values[addressId]!);
        }

        if (values.Contains(director))
        {
            model.Director = Convert.ToString(values[director])!;
        }

        if (values.Contains(description))
        {
            model.Description = Convert.ToString(values[description])!;
        }
    }

    protected void PopulateModel(RentToCreateDto model, IDictionary values)
    {
        string tenantId = nameof(RentToCreateDto.TenantId);
        string assetId = nameof(RentToCreateDto.AssetId);
        string startDate = nameof(RentToCreateDto.StartDate);
        string endDate = nameof(RentToCreateDto.EndDate);

        if (values.Contains(tenantId))
        {
            model.TenantId = ConvertTo<Guid>(values[tenantId]!);
        }

        if (values.Contains(assetId))
        {
            model.AssetId = ConvertTo<Guid>(values[assetId]!);
        }

        if (values.Contains(startDate))
        {
            model.StartDate = Convert.ToDateTime(values[startDate]!);
        }

        if (values.Contains(endDate))
        {
            model.EndDate = Convert.ToDateTime(values[endDate]!);
        }
    }

    protected void PopulateModel(SignUpUser model, IDictionary values)
    {
        string email = nameof(SignUpUser.Email);
        string password = nameof(SignUpUser.Password);
        string phoneNumber = nameof(SignUpUser.PhoneNumber);

        if (values.Contains(email))
        {
            model.Email = Convert.ToString(values[email]!)!;
        }

        if (values.Contains(password))
        {
            model.Password = Convert.ToString(values[password]!)!;
        }

        if (values.Contains(phoneNumber))
        {
            model.PhoneNumber = Convert.ToString(values[phoneNumber]!)!;
        }
    }
}