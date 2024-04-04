using Microsoft.Data.SqlClient;
using Rent.ADO.NET.DTOs;
using Rent.ADO.NET.Services.Contracts;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace Rent.ADO.NET.Services;

public class ConnectedArchitecture(IConfiguration configuration) : IConnectedArchitecture
{
    public async Task<IEnumerable<AddressDto>> GetAllAddressesAsync()
    {
        var query = "SELECT * FROM [dbo].[Address]";

        await using var connection = new SqlConnection(configuration["ConnectionStrings:RentDatabase"]);

        var command = new SqlCommand(query, connection);

        await connection.OpenAsync();

        var reader = await command.ExecuteReaderAsync();
        var addresses = new List<AddressDto>();

        if (reader.HasRows)
        {
            while (await reader.ReadAsync())
            {
                var address = new AddressDto
                {
                    AddressId = reader.GetGuid(0),
                    City = reader.GetString(1),
                    Building = reader.GetString(2),
                    Street = reader.GetString(3)
                };

                addresses.Add(address);
            }
        }
        reader.Close();

        return addresses;
    }

    public async Task<AddressDto?> GetAddressByIdAsync(Guid addressId)
    {
        var query = $"SELECT * FROM [dbo].[Address] WHERE [AddressId] = '{addressId}'";

        await using var connection = new SqlConnection(configuration["ConnectionStrings:RentDatabase"]);

        var command = new SqlCommand(query, connection);

        await connection.OpenAsync();

        var reader = await command.ExecuteReaderAsync();
        AddressDto? address = null;

        if (reader.HasRows)
        {
            while (await reader.ReadAsync())
            {
                address = new AddressDto
                {
                    AddressId = reader.GetGuid(0),
                    City = reader.GetString(1),
                    Building = reader.GetString(2),
                    Street = reader.GetString(3)
                };
            }
        }

        reader.Close();

        return address;
    }
}