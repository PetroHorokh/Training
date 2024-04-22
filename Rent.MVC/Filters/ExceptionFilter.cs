using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Rent.MVC.Models;

namespace Rent.MVC.Filters;

public class CustomExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var error = new ErrorModel(statusCode: 500, message: context.Exception.Message, details: context.Exception.InnerException?.Message);

        var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
        {
            { "StatusCode", error.StatusCode },
            { "Message", error.Message },
            { "Details", error.Details }
        };

        context.Result = new ViewResult
        {
            ViewName = "Error",
            ViewData = viewData
        };

        context.ExceptionHandled = true;
    }
}