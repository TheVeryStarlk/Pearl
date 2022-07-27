using Microsoft.AspNetCore.SignalR;
using Pearl.Api.Extensions;
using Pearl.Api.Services;
using Pearl.Models.Responses;

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

        return response.IfSuccess(async () =>
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, name);
            await Clients.Group(name).SendAsync("Group", response.Value);
        });
    }

    [HubMethodName("SendMessage")]
    public async Task<ErrorResponse?> SendMessageAsync(string content, string groupName)
    {
        var subject = Context.Subject();
        var response = await pearlService.SendMessageAsync(content, groupName, subject);

        return response.IfSuccess(async () =>
            await Clients.Group(groupName).SendAsync("Message", subject, response.Value));
    }
}