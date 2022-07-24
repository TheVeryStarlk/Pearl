using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Pearl.Api.Extensions;

public static class ContextExtensions
{
    public static string Subject(this HubCallerContext context)
    {
        return context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
    }
}