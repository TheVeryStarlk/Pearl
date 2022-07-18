using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Pearl.Api.Models.Responses;

namespace Pearl.Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        return result.IsSuccess
            ? new OkObjectResult(result.Value)
            : new BadRequestObjectResult(new ErrorResponse(result.Errors.Select(error => error.Message).ToArray()));
    }
}