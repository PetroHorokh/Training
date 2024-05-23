using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;
using Amazon.S3.Model;
using Amazon.SimpleNotificationService;
using System.Text.Json;
using Amazon.SimpleNotificationService.Model;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Rent.AWS.Lambda.TenantBillStatus;

public class Function
{
    public void FunctionHandler(PublishBatchResponse test)
    {
        var sns = new AmazonSimpleNotificationServiceClient(Amazon.RegionEndpoint.EUNorth1);

        if (test.Failed.Count == 0)
        {
            sns.PublishAsync("arn:aws:sns:eu-north-1:992382749720:TenantBillStatus", "Successfully created a dump");
        }
        else
        {
            throw new Exception("DynamoDB insert was not successful");
        }
    }
}
