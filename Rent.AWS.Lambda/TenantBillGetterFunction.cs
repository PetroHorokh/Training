using System.Globalization;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Rent.AWS.Lambda.TenantBillGetter.Models;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Rent.AWS.Lambda.TenantBillGetter;

public class TenantBillGetterFunction
{
    private readonly HttpClient _client = new();
    private readonly IAmazonDynamoDB _dynamoDbClient = new AmazonDynamoDBClient(RegionEndpoint.EUNorth1);
    private readonly string _url = "http://rentwebapi-dev.eba-nhhj2vtd.eu-north-1.elasticbeanstalk.com/Tenant/GetAllBills";

    public async Task FunctionHandler()
    {
        try
        {
            HttpResponseMessage response = await _client.GetAsync(_url);
            response.EnsureSuccessStatusCode();

            string data = await response.Content.ReadAsStringAsync();

            var parsedData = ParseResponseData(data);

            if (parsedData is null)
            {
                throw new Exception("No retrieved tenant bills");
            }

            await PutItemsInDynamoDb(parsedData);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to process request: {ex.Message}");
        }
    }

    private async Task PutItemsInDynamoDb(List<Bill> data)
    {
        DynamoDBContext context = new DynamoDBContext(_dynamoDbClient);

        var bookBatch = context.CreateBatchWrite<Bill>();
        bookBatch.AddPutItems(data);

        await bookBatch.ExecuteAsync();
    }

    private List<Bill>? ParseResponseData(string data)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };

        List<Bill>? deserializedList = JsonConvert.DeserializeObject<List<Bill>>(data, settings);

        return deserializedList;
    }

    private static AttributeValue ConvertToAttributeValue(object value)
    {
        switch (value)
        {
            case string s:
                return new AttributeValue { S = s };
            case int i:
                return new AttributeValue { N = i.ToString() };
            case long l:
                return new AttributeValue { N = l.ToString() };
            case double d:
                return new AttributeValue { N = d.ToString(CultureInfo.CurrentCulture) };
            case bool b:
                return new AttributeValue { BOOL = b };
            case DateTime dt:
                return new AttributeValue { S = dt.ToString("o") };
            default:
                throw new ArgumentException($"Unsupported property type: {value.GetType().Name}");
        }
    }
}
