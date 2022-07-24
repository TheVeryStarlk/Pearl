using Microsoft.AspNetCore.SignalR;
using Pearl.Api.Extensions;
using Pearl.Api.Models.Responses;
using Pearl.Api.Services;

namespace Pearl.Api.Hubs;

public sealed class GroupsHub : Hub
{
    private readonly GroupsService groupsService;

    public GroupsHub(GroupsService groupsService)
    {
        this.groupsService = groupsService;
    }

    [HubMethodName("JoinGroup")]
    public async Task<ErrorResponse?> JoinGroupAsync(string name)
    {
        var response = await groupsService.JoinGroupAsync(name, Context.Subject());

        if (response.IsSuccess)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, name);
            await Clients.Group(name).SendAsync("Group", response.Value);

            return null;
        }

        return new ErrorResponse(response.Errors.Select(error => error.Message).ToArray());
    }
}