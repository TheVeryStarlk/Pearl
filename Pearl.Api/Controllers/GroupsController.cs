using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pearl.Api.Extensions;
using Pearl.Api.Services;
using Pearl.Models;
using Pearl.Models.Responses;
using System.Security.Claims;

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

    [HttpGet]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string[]))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ErrorResponse))]
    public IActionResult Groups()
    {
        var response = groupsService.Groups(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        return response.ToActionResult();
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