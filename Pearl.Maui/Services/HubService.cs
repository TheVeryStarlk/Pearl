using FluentResults;
using Microsoft.AspNetCore.SignalR.Client;
using Pearl.Models.Responses;

namespace Pearl.Maui.Services;

public sealed class HubService
{
    public Action? Group;

    public Action<string, string, string>? Message;

    public Action<string>? GroupMessage;

    private readonly HubConnection connection;
    private readonly AuthenticationService authenticationService;

    public HubService(AuthenticationService authenticationService)
    {
        this.authenticationService = authenticationService;

        connection = new HubConnectionBuilder()
            .WithUrl($"{Preferences.Get("Url", null)!}/pearlHub", options => options.AccessTokenProvider = ProvideAccessTokenAsync)
            .WithAutomaticReconnect()
            .Build();
    }

    public async Task StartAsync()
    {
        if (connection.State is not HubConnectionState.Connected or HubConnectionState.Connecting)
        {
            await connection.StartAsync();
        }

        connection.On<string>("Group", message => GroupMessage?.Invoke(message));
        connection.On<string, string, string>("Message", (userName, groupName, message) => Message?.Invoke(userName, groupName, message));
    }

    public async Task<Result<string>> JoinGroupAsync(string name)
    {
        var request = await connection.InvokeAsync<ErrorResponse?>("JoinGroup", name);

        if (request is null)
        {
            Group?.Invoke();
            return Result.Ok("You have joined the group successfully.");
        }

        return Result.Fail(request.Errors);
    }

    public async Task<Result> SendMessageAsync(string content, string groupName)
    {
        var request = await connection.InvokeAsync<ErrorResponse?>("SendMessage", content, groupName);

        return request is null
            ? Result.Ok()
            : Result.Fail(request.Errors);
    }

    private async Task<string?> ProvideAccessTokenAsync()
    {
        var response = await authenticationService.RefreshAsync(Preferences.Get("AccessToken", null));
        return response.Value.AccessToken;
    }
}
