using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Rent.ADO.NET.DTOs;
using Rent.ADO.NET.Services.Contracts;

namespace Rent.ADO.NET.Services;

public class DisconnectedArchitecture(IConfiguration configuration) : IDisconnectedArchitecture
{
    public async Task<IEnumerable<AvailableAssetDto>> GetAvailableAssetsAsync(DateTime dateTime)
    {
        var query = $"SELECT DISTINCT [Asset].[AssetId] " +
                    $"FROM [dbo].[Asset] [Asset] " +
                    $"LEFT JOIN [dbo].[Rent] AS [Rent] ON [Rent].[AssetId] = [Asset].[AssetId] " +
                    $"WHERE '{dateTime}' NOT BETWEEN [Rent].[StartDate] AND [Rent].[EndDate] " +
                    $"UNION " +
                    $"SELECT DISTINCT [Asset].[AssetId] " +
                    $"FROM [dbo].[Asset] [Asset] " +
                    $"LEFT JOIN [dbo].[Rent] AS [Rent] ON [Rent].[AssetId] = [Asset].[AssetId] " +
                    $"WHERE [Rent].[RentId] IS NULL";
        await using var connection = new SqlConnection(configuration["ConnectionStrings:RentDatabase"]);

        var command = new SqlCommand(query, connection);

        await connection.OpenAsync();

        var adapter = new SqlDataAdapter(command);
        var dataSet = new DataSet();
        adapter.Fill(dataSet, "EmptyAssets");

        await connection.CloseAsync();

        var rows = dataSet.Tables["EmptyAssets"]!.Rows;

        var assets = new List<AvailableAssetDto>();

        foreach (DataRow row in rows)
        {
            var asset = new AvailableAssetDto()
            {
                AssetId = Guid.Parse(row.ItemArray[0]!.ToString()!)
            };
            assets.Add(asset);
        }

        return assets;
    }

    public async Task<IEnumerable<AssetBookingDto>> GetAssetBookingAsync(Guid assetId)
    {
        var query = $"SELECT *" +
                    $"FROM [dbo].[Rent] [Rent]" +
                    $"WHERE [Rent].[AssetId] = '{assetId}' AND [Rent].[EndDate] > GETDATE()";
        await using var connection = new SqlConnection(configuration["ConnectionStrings:RentDatabase"]);

        var command = new SqlCommand(query, connection);

        await connection.OpenAsync();

        var adapter = new SqlDataAdapter(command);
        var dataSet = new DataSet();
        adapter.Fill(dataSet, "Booking");

        await connection.CloseAsync();

        var rows = dataSet.Tables["Booking"]!.Rows;

        var rents = new List<AssetBookingDto>();

        foreach (DataRow row in rows)
        {
            var asset = new AssetBookingDto
            {
                RentId = Guid.Parse(row["RentId"].ToString()!),
                StartDate = DateTime.Parse(row["StartDate"].ToString()!),
                EndDate = DateTime.Parse(row["EndDate"].ToString()!),
            };
            rents.Add(asset);
        }

        return rents;
    }
}