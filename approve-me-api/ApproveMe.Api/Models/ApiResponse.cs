using ApproveMe.Api.Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ApproveMe.Api.Models;

public class ApiResponse<T>
{
    public bool Success { get; set; } = true;
    public string? Message { get; set; }
    public T? Data { get; set; } = default;

    public ApiResponse()
    {

    }

    public ApiResponse(bool success, T? data, string message)
    {
        Success = success;
        Message = message;
        Data = data;
    }

    public ApiResponse<T> SuccessResponse(T data)
    {
        return new ApiResponse<T>(true, data, "Success.");
    }

    public ApiResponse<T> SuccessResponse(T data, string message)
    {
        return new ApiResponse<T>(true, data, message);
    }

    public ApiResponse<T> Fail(string message)
    {
        return new ApiResponse<T>(false, default, message);
    }
    
    public ApiResponse<T> Unauthorized()
    {
        return new ApiResponse<T>(false, default, ResponseConstant.UNAUTHORIZED_MESSAGE);
    }
    
    public ApiResponse<T> Unauthorized(string message)
    {
        return new ApiResponse<T>(false, default, message);
    }

    public ApiResponse<T> SessionExpired()
    {
        return new ApiResponse<T>(false, default, ResponseConstant.SESSION_EXPIRED_MESSAGE);
    }

    public ApiResponse<T> Forbidden()
    {
        return new ApiResponse<T>(false, default, ResponseConstant.FORBIDDEN_MESSAGE);
    }
    
    public override string ToString()
    {
        DefaultContractResolver contractResolver = new()
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        return JsonConvert.SerializeObject(this, new JsonSerializerSettings
        {
            ContractResolver = contractResolver,
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore,
        });
    }
}