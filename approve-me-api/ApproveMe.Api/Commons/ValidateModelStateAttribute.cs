using ApproveMe.Api.Constants;
using ApproveMe.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApproveMe.Api.Commons;

public class ValidateModelStateAttribute : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is not HttpResponseException)
        {
            return;
        };
        
        context.Result = new ObjectResult(new ApiResponse<object>
        {
            Success = false,
            Message = ResponseConstant.BAD_REQUEST_MESSAGE,
            Data = context.ModelState
        });
        context.ExceptionHandled = true;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.Result = new ObjectResult(new ApiResponse<object>
            {
                Success = false,
                Message = ResponseConstant.BAD_REQUEST_MESSAGE,
                Data = context.ModelState
            });
        }
    }
}

public abstract class HttpResponseException : Exception
{
    public int Status { get; set; } = 500;
    public object? Value { get; set; } = default;
}