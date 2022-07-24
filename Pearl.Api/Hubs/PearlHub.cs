using Microsoft.AspNetCore.SignalR;
using Pearl.Api.Extensions;
using Pearl.Api.Models.Responses;
using Pearl.Api.Services;

namespace Pearl.Api.Hubs;

public sealed class PearlHub : Hub
{
    private readonly PearlService pearlService;

    public PearlHub(PearlService pearlService)
    {
        this.pearlService = pearlService;
    }
    
    [HubMethodName("JoinGroup")]
    public async Task<ErrorResponse?> JoinGroupAsync(string name)
    {
        var response = await pearlService.JoinGroupAsync(name, Context.Subject());

        if (response.IsSuccess)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, name);
            await Clients.Group(name).SendAsync("Group", response.Value);

            return null;
        }

        return new ErrorResponse(response.Errors.Select(error => error.Message).ToArray());
    }
    
    [HubMethodName("SendMessage")]
    public async Task<ErrorResponse?> SendMessageAsync(string content, string groupName)
    {
        var subject = Context.Subject();
        var response = pearlService.SendMessage(content, groupName, subject);

        if (response.IsSuccess)
        {
            await Clients.Group(groupName).SendAsync("Message", subject, response.Value);
            return null;
        }

        return new ErrorResponse(response.Errors.Select(error => error.Message).ToArray());
    }
}