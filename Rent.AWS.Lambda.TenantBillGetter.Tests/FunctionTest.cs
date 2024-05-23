using Amazon.Lambda.Core;

namespace Rent.AWS.Lambda.Tests;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

public class FunctionTest
{
    //[Fact]
    //public void TestToUpperFunction()
    //{

    //    // Invoke the lambda function and confirm the string was upper cased.
    //    var function = new Function();
    //    var context = new TestLambdaContext();
    //    var upperCase = function.FunctionHandler();

    //    Assert.Equal("HELLO WORLD", upperCase);
    //}
}
