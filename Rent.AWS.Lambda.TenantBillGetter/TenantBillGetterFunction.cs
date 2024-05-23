using System.Globalization;
using System.Reflection;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Rent.AWS.Lambda.Models;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Rent.AWS.Lambda;

public class TenantBillGetterFunction
{
    private readonly HttpClient _client = new();
    private readonly IAmazonDynamoDB _dynamoDbClient = new AmazonDynamoDBClient(RegionEndpoint.EUNorth1);
    private readonly string _dynamoDbTableName = "TenantBills";
    private readonly string _url = "https://localhost:7147/Tenant/GetAllBills";

    public async Task<string> FunctionHandler()
    {
        try
        {
            HttpResponseMessage response = await _client.GetAsync(_url);
            response.EnsureSuccessStatusCode();

            string data = await response.Content.ReadAsStringAsync();

            var parsedData = ParseResponseData(data);

            if (parsedData is not null)
            {
                await PutItemsInDynamoDb(parsedData);

                return "Data successfully retrieved and inserted into DynamoDB";
            }

            return "Could not work with retrieved data from request";
        }
        catch (Exception ex)
        {
            return $"Failed to process request: {ex.Message}";
        }
    }

    private async Task PutItemsInDynamoDb(List<Bill> data)
    {
        DynamoDBContext context = new DynamoDBContext(_dynamoDbClient);

        var bookBatch = context.CreateBatchWrite<Bill>();
        bookBatch.AddPutItems(data);

        await bookBatch.ExecuteAsync();

        //foreach (var obj in data)
        //{
        //    var attributes = ConvertObjectToAttributes(obj);

        //    var putItemRequest = new PutItemRequest
        //    {
        //        TableName = _dynamoDbTableName,
        //        Item = attributes
        //    };

        //    await _dynamoDbClient.PutItemAsync(putItemRequest);
        //}
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

    private Dictionary<string, AttributeValue> ConvertObjectToAttributes(object data)
    {
        var attributeValueMap = new Dictionary<string, AttributeValue>();
        var properties = data.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var name = property.Name;
            var value = property.GetValue(data)!.ToString();

            if (value != null)
            {
                AttributeValue attributeValue = ConvertToAttributeValue(value);
                attributeValueMap[name] = attributeValue;
            }
        }

        return attributeValueMap;
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