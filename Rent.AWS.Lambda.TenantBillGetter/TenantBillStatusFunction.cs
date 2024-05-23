using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;

namespace Rent.AWS.Lambda;

//public class TenantBillStatus
//{
//    public void FunctionHandler(DynamoDBEvent dynamoEvent, ILambdaContext context)
//    {
//        context.Logger.LogInformation($"Beginning to process {dynamoEvent.Records.Count} records...");

//        foreach (var record in dynamoEvent.Records)
//        {
//            context.Logger.LogInformation($"Event ID: {record.EventID}");
//            context.Logger.LogInformation($"Event Name: {record.EventName}");


//        }

//        context.Logger.LogInformation("Stream processing complete.");
//    }
//}