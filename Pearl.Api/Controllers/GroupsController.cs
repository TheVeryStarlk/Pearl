using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pearl.Api.Extensions;
using Pearl.Api.Models;
using Pearl.Api.Models.Responses;
using Pearl.Api.Services;

namespace Pearl.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public sealed class GroupsController : ControllerBase
{
    private readonly GroupsService groupsService;

    public GroupsController(GroupsService groupsService)
    {
        this.groupsService = groupsService;
    }

    [HttpGet("messages")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(Message[]))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ErrorResponse))]
    public IActionResult Messages(string groupName)
    {
        var response = groupsService.Messages(groupName, HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        return response.ToActionResult();
    }
}