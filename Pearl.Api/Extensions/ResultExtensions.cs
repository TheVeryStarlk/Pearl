using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Pearl.Models;

namespace Pearl.Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        return result.IsSuccess
            ? new OkObjectResult(result.Value)
            : new BadRequestObjectResult(new ErrorResponse(result.Errors.Select(error => error.Message).ToArray()));
    }

    public static ErrorResponse? IfSuccess<TResult>(this Result<TResult> result, Func<Task> func)
    {
        if (result.IsSuccess)
        {
            func();
            return null;
        }

        return new ErrorResponse(result.Errors.Select(error => error.Message).ToArray());
    }
}