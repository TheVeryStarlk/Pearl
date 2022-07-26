using Microsoft.AspNetCore.Mvc;
using Pearl.Api.Extensions;
using Pearl.Api.Models.Requests;
using Pearl.Api.Models.Responses;
using Pearl.Api.Services;
using Pearl.Models;

namespace Pearl.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class AuthenticationController : ControllerBase
{
    private readonly AuthenticationService authenticationService;

    public AuthenticationController(AuthenticationService authenticationService)
    {
        this.authenticationService = authenticationService;
    }

    [HttpPost("authenticate")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(AuthenticateResponse))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ErrorResponse))]
    public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticateRequest request)
    {
        var response = await authenticationService.AuthenticateAsync(request);
        return response.ToActionResult();
    }

    [HttpPost("refresh")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(RefreshResponse))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ErrorResponse))]
    public IActionResult Refresh([FromBody] RefreshRequest request)
    {
        var response = authenticationService.Refresh(request);
        return response.ToActionResult();
    }
}