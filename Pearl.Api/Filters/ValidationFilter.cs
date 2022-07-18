using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pearl.Api.Models.Responses;

namespace Pearl.Api.Filters;

public sealed class ValidationFilter : IAsyncActionFilter
{
    // Kudos to Nick Chapsas!
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(entry => entry.Value?.Errors.Count > 0)
                .ToDictionary(selector => selector.Key,
                    selector => selector.Value?.Errors.Select(error => error.ErrorMessage));

            var messages = new List<string>();

            foreach (var error in errors)
            {
                if (error.Value is null)
                {
                    continue;
                }

                messages.AddRange(error.Value.Select(message => message));
            }

            context.Result = new BadRequestObjectResult(new ErrorResponse(messages.ToArray()));
            return;
        }

        await next();
    }
}