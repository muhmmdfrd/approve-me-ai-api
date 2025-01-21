using ApproveMe.Api.Commons;
using ApproveMe.Api.Models;
using Flozacode.Models.Paginations;
using ApproveMe.Core.Dtos;
using ApproveMe.Core.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApproveMe.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class RolesController : FlozaApiController
{
    private readonly RoleHelper _helper;

    public RolesController(RoleHelper helper)
    {
        _helper = helper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<Pagination<RoleViewDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPaged([FromQuery] RoleFilter filter)
    {
        var result = await _helper.GetPagedAsync(filter);
        return ApiOK(result);
    }
    
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<RoleViewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Find([FromRoute] long id)
    {
        var result = await _helper.FindAsync(id);
        return ApiOK(result);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] RoleAddDto dto)
    {
        var affected = await _helper.CreateAsync(dto, CurrentUser);
        if (affected <= 0)
        {
            return ApiDataInvalid("Role not created.");
        }

        return ApiCreated();
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromBody] RoleUpdDto dto)
    {
        var affected = await _helper.UpdateAsync(dto, CurrentUser);
        if (affected <= 0)
        {
            return ApiDataInvalid("Role not updated.");
        }

        return ApiOK("Role updated.");
    }
    
    [HttpDelete("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([FromRoute] long id)
    {
        var affected = await _helper.DeleteAsync(id, CurrentUser, false);
        if (affected <= 0)
        {
            return ApiDataInvalid("Role not deleted.");
        }

        return ApiOK("Role deleted.");
    }
}