using ApproveMe.Api.Commons;
using ApproveMe.Api.Models;
using ApproveMe.Core.Constants;
using ApproveMe.Core.Dtos;
using ApproveMe.Core.Helpers;
using ApproveMe.Core.Services.RedisCaching;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApproveMe.Api.Controllers;

[ApiController]
[Route("[controller]")]
[AllowAnonymous]
public class AuthController(AuthHelper authHelper, RedisService redisService) : FlozaApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetSession()
    {
        var sessions = await redisService.HashGetAllAsync(RedisConstant.UserSession);
        return ApiOK(sessions);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] AuthRequestDto request)
    {
        var result = await authHelper.AuthAsync(request);
        if (result == null)
        {
            return ApiUnauthorized();
        }
        
        return ApiOK(result);
    }
    
    [HttpPost("revoke")]
    [ProducesResponseType(typeof(ApiResponse<AuthRevokeResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Revoke([FromBody] AuthRevokeRequestDto request)
    {
        var result = await authHelper.RevokeAsync(request);
        if (result == null)
        {
            return ApiUnauthorized();
        }
        
        return ApiOK(result);
    }
    
    [HttpPost("logout")]
    [ProducesResponseType(typeof(ApiResponse<AuthRevokeResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout([FromBody] LogoutRequestDto request)
    {
        var result = await authHelper.LogoutAsync(request);
        if (result)
        {
            return ApiOK("User logged out.");
        }

        return ApiDataInvalid("Logout failed.");
    }
}