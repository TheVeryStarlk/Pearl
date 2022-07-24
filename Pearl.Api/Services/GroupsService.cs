using FluentResults;
using Microsoft.EntityFrameworkCore;
using Pearl.Database;
using Pearl.Database.Models;

namespace Pearl.Api.Services;

public sealed class GroupsService
{
    private readonly PearlContext pearlContext;

    public GroupsService(PearlContext pearlContext)
    {
        this.pearlContext = pearlContext;
    }

    public async Task<Result<string>> JoinGroupAsync(string groupName, string userName)
    {
        var requestedGroup = pearlContext.Groups.FirstOrDefault(group => group.Name == groupName);
        var requestedUser = pearlContext.Users.First(user => user.Name == userName);
        var groups = requestedUser.Groups;

        if (requestedGroup is null)
        {
            var group = new Group()
            {
                Name = groupName,
                Users = new List<User>()
                {
                    requestedUser
                }
            };

            if (groups is not null)
            {
                groups.Add(group);
            }
            else
            {
                groups = new List<Group>()
                {
                    group
                };
            }

            pearlContext.Groups.Add(group);
            await pearlContext.SaveChangesAsync();

            return Result.Ok($"\'{userName}\' has joined the group.");
        }

        if (groups is not null)
        {
            groups.Add(requestedGroup);
        }
        else
        {
            groups = new List<Group>()
            {
                requestedGroup
            };
        }

        await pearlContext.SaveChangesAsync();

        var databaseGroup = pearlContext.Groups.Include(path => path.Users).First(group => group.Name == groupName);

        return databaseGroup.Users.Any(user => user.Name == userName)
            ? Result.Fail($"\'{userName}\' is already in the group.")
            : Result.Ok($"\'{userName}\' has joined the group.");
    }
}