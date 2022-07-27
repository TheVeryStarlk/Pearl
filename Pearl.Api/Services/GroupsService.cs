using FluentResults;
using Microsoft.EntityFrameworkCore;
using Pearl.Database;
using Pearl.Models;

namespace Pearl.Api.Services;

public sealed class GroupsService
{
    private readonly PearlContext pearlContext;

    public GroupsService(PearlContext pearlContext)
    {
        this.pearlContext = pearlContext;
    }

    public Result<string[]> Groups(string userName)
    {
        var user = pearlContext.Users.Include(path => path.Groups).First(user => user.Name == userName);

        return user.Groups is null || user.Groups.Count == 0
            ? Result.Fail($"'{userName}' is not in any group.")
            : user.Groups.Select(group => group.Name).ToArray();
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

            var messages = pearlContext.Messages.Where(message => message.Group.Name == groupName);
            return Result.Ok(messages.Select(message => new Message(message.User.Name, message.Content)).ToArray());
        }

        return Result.Fail("The requested group does not exist.");
    }
    
    public Result<string[]> Users(string groupName, string userName)
    {
        var group = pearlContext.Groups.Include(path => path.Users).FirstOrDefault(group => group.Name == groupName);

        if (group is not null)
        {
            if (group.Users.All(user => user.Name != userName))
            {
                return Result.Fail($"'{userName}' is not in the requested group.");
            }

            return group.Users.Select(user => user.Name).ToArray();
        }

        return Result.Fail("The requested group does not exist.");
    }
}