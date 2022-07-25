using FluentResults;
using Microsoft.EntityFrameworkCore;
using Pearl.Api.Models;
using Pearl.Database;

namespace Pearl.Api.Services;

public sealed class GroupsService
{
    private readonly PearlContext pearlContext;

    public GroupsService(PearlContext pearlContext)
    {
        this.pearlContext = pearlContext;
    }

    public Result<Message[]> Messages(string groupName, string userName)
    {
        var group = pearlContext.Groups.Include(path => path.Users).FirstOrDefault(group => group.Name == groupName);

        if (group is not null)
        {
            if (group.Users.All(user => user.Name != userName))
            {
                return Result.Fail($"'{userName}' is not in the requested group.");
            }

            var messages = pearlContext.Messages.Where(message => message.Group.Name == groupName).ToArray();
            return Result.Ok(messages.Select(message => new Message(message.User.Name, message.Content)).ToArray());
        }

        return Result.Fail("The requested group does not exist.");
    }
}