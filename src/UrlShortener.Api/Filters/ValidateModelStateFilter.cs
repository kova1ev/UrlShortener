using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UrlShortener.Api.Models;
using UrlShortener.Application.Common.Constants;

namespace UrlShortener.Api.Filters;

public class ValidateModelStateFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // for bad format request  

        if (context.ModelState.IsValid)
        {
            return;
        }

        var errors = context.ModelState.SelectMany(e => e.Value.Errors.Select(m => m?.ErrorMessage));
        var apiErrors = new ApiErrors(StatusCodes.Status400BadRequest, StatusCodeErrorMessage.BadRequestErrorMessage,
            errors);

        context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Result = new ObjectResult(apiErrors);
    }
}