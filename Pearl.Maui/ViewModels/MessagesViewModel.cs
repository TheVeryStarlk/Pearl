using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Pearl.Maui.Messages;
using Pearl.Maui.Services;
using Pearl.Models;
using System.Collections.ObjectModel;

namespace Pearl.Maui.ViewModels;

public sealed class MessagesViewModel : ObservableObject, IQueryAttributable
{
    public string? Group
    {
        get => group;
        set => SetProperty(ref group, value);
    }

    public ObservableCollection<Message> Messages
    {
        get => messages;
        set => SetProperty(ref messages, value);
    }

    public AsyncRelayCommand<string?> SendMessageCommandAsync { get; }

    private string? group;
    private ObservableCollection<Message> messages;

    private readonly AuthenticationService authenticationService;
    private readonly HubService hubService;

    public MessagesViewModel(AuthenticationService authenticationService, HubService hubService)
    {
        this.authenticationService = authenticationService;
        this.hubService = hubService;

        hubService.GroupMessage += async (message) => await Toast.Make(message).Show();
        hubService.Message += HandleNewMessage;

        messages = new ObservableCollection<Message>();

        SendMessageCommandAsync = new AsyncRelayCommand<string?>(SendMessageAsync);
    }

    private async Task SendMessageAsync(string? message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        await hubService.SendMessageAsync(message, group);
    }

    private void HandleNewMessage(string userName, string groupName, string message)
    {
        Messages.Add(new Message(userName, message));
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Group = (string)query["group"];

        var messages = await authenticationService.MessagesAsync(Group);

        if (messages.IsSuccess)
        {
            Messages = new ObservableCollection<Message>(messages.Value);
        }
    }
}