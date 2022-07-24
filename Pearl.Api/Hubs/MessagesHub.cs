using Microsoft.AspNetCore.SignalR;
using Pearl.Api.Extensions;
using Pearl.Api.Models.Responses;
using Pearl.Api.Services;

namespace Pearl.Api.Hubs;

public sealed class MessagesHub : Hub
{
    private readonly MessagesService messagesService;

    public MessagesHub(MessagesService messagesService)
    {
        this.messagesService = messagesService;
    }

    [HubMethodName("SendMessage")]
    public async Task<ErrorResponse?> SendMessageAsync(string content, string groupName)
    {
        var subject = Context.Subject();
        var response = messagesService.SendMessage(content, groupName, subject);

        if (response.IsSuccess)
        {
            await Clients.Group(groupName).SendAsync("Message", subject, response.Value);
            return null;
        }

        return new ErrorResponse(response.Errors.Select(error => error.Message).ToArray());
    }
}