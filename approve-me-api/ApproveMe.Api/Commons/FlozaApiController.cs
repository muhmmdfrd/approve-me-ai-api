using System.Net;
using ApproveMe.Api.Constants;
using ApproveMe.Api.Models;
using ApproveMe.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApproveMe.Api.Commons;

public class FlozaApiController : ControllerBase
{
    private readonly IHttpContextAccessor _contextAccessor;

    public FlozaApiController() 
    {
        _contextAccessor = new HttpContextAccessor();
    }

    public FlozaApiController(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }
    
    [NonAction]
    protected ObjectResult ApiOK()
    {
        var response = new ApiResponse<object?>().SuccessResponse(null, ResponseConstant.SUCCESS_MESSAGE);
        return Ok(response);
    }

    [NonAction]
    protected ObjectResult ApiOK<T>(T data)
    {
        var response = new ApiResponse<T>().SuccessResponse(data);
        return Ok(response);
    }

    [NonAction]
    public ObjectResult ApiOK(string message)
    {
        var response = new ApiResponse<object?>().SuccessResponse(null, message);
        return Ok(response);
    }

    [NonAction]
    public ObjectResult ApiCreated<T>(T data)
    {
        var response = new ApiResponse<object?>().SuccessResponse(data, ResponseConstant.CREATED_MESSAGE);
        return StatusCode((int)HttpStatusCode.Created, response);
    }
    
    [NonAction]
    public ObjectResult ApiCreated()
    {
        var response = new ApiResponse<object?>().SuccessResponse(null, ResponseConstant.CREATED_MESSAGE);
        return StatusCode((int)HttpStatusCode.Created, response);
    }
    
    [NonAction]
    protected ObjectResult ApiDataInvalid(string? message = null)
    {
        return StatusCode((int)HttpStatusCode.BadRequest, new ApiResponse<object?>().Fail(message ?? ResponseConstant.FAILED_MESSAGE));
    }

    [NonAction]
    protected ObjectResult ApiUnauthorized()
    {
        var response = new ApiResponse<object?>().Unauthorized();
        return StatusCode((int)HttpStatusCode.Unauthorized, response);
    }

    [NonAction]
    protected ObjectResult ApiForbidden()
    {
        var response = new ApiResponse<object?>().Forbidden();
        return StatusCode((int)HttpStatusCode.Forbidden, response);
    }
    
    protected string GetBaseUrl()
    {
        var request = Request;
        var host = request.Host.ToUriComponent();
        var pathBase = request.PathBase.ToUriComponent();
        return $"{request.Scheme}://{host}{pathBase}";
    }

    protected virtual CurrentUser CurrentUser
    {
        get
        {
            var user = new CurrentUser
            {
                Id = int.Parse(GetClaim("Id") ?? "1"),
                Username = GetClaim("Username") ?? "",
                Name = GetClaim("Name") ?? "",
            };

            return user;
        }
    }

    private string? GetClaim(string type)
    {
        var claim = _contextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == type);
        return claim?.Value;
    }
}