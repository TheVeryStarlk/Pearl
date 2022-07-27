using CommunityToolkit.Mvvm.ComponentModel;
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

    public ObservableCollection<Message> Messages { get; private set; }

    private string? group;

    private readonly AuthenticationService authenticationService;
    private readonly HubService hubService;

    public MessagesViewModel(AuthenticationService authenticationService, HubService hubService)
    {
        this.authenticationService = authenticationService;
        this.hubService = hubService;

        Messages = new ObservableCollection<Message>();

        WeakReferenceMessenger.Default.Register<MessagesRequest>(this,
            async (_, message) => await UpdateMessagesAsync(message));
    }

    private async Task UpdateMessagesAsync(MessagesRequest message)
    {
        var messages = await authenticationService.MessagesAsync(message.GroupName);

        if (messages.IsSuccess)
        {
            Messages = new ObservableCollection<Message>(messages.Value);
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Group = (string)query["group"];
    }
}