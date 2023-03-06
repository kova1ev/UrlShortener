using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UrlShortener.Application.Common.Constants;

namespace UrlShortener.Api.Filters;

public class ValidateModelStateFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {

    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid == false)
        {
            //Dictionary<string, IEnumerable<string>> errors = new Dictionary<string, IEnumerable<string>>();
            //foreach (var item in context.ModelState)
            //{
            //    string key = item.Key;
            //    ModelErrorCollection errorsCollections = item.Value.Errors;
            //    if (errorsCollections != null && errorsCollections.Count > 0)
            //        errors[key] = errorsCollections.Select(e => e.ErrorMessage).ToArray();
            //}

            var errors = context.ModelState.SelectMany(e => e.Value.Errors.Select(m => m.ErrorMessage));
            ApiErrors apiErrors = new ApiErrors(StatusCodes.Status400BadRequest, StatusCodeMessage.BAD_REQUEST_MESSAGE, errors);

            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Result = new ObjectResult(apiErrors);
        }
    }
}
