using Rent.ADO.NET.DTOs;

namespace Rent.ADO.NET.Services.Contracts;

public interface IConnectedArchitecture
{
    Task<IEnumerable<AddressDto>> GetAllAddressesAsync();

    Task<AddressDto?> GetAddressByIdAsync(Guid addressId);
}