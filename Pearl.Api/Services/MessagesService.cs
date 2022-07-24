using FluentResults;
using Microsoft.EntityFrameworkCore;
using Pearl.Database;

namespace Pearl.Api.Services;

public sealed class MessagesService
{
    private readonly PearlContext pearlContext;

    public MessagesService(PearlContext pearlContext)
    {
        this.pearlContext = pearlContext;
    }

    public Result<string> SendMessage(string content, string groupName, string userName)
    {
        var databaseGroup = pearlContext.Groups.Include(path => path.Users).First(group => group.Name == groupName);

        return databaseGroup.Users.Any(user => user.Name == userName)
            ? Result.Ok(content)
            : Result.Fail("Failed to send a message to the requested group.");
    }
}