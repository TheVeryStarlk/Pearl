using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Pearl.Maui.Messages;
using Pearl.Maui.Services;
using Pearl.Maui.Views;
using System.Diagnostics;

namespace Pearl.Maui.ViewModels;

public sealed class GroupsViewModel : ObservableObject
{
    public string[]? Groups
    {
        get => groups;
        set => SetProperty(ref groups, value);
    }

    private string[]? groups;

    public AsyncRelayCommand<string?> OpenGroupCommandAsync { get; }

    public HubService HubService { get; }

    private readonly AuthenticationService authenticationService;

    public GroupsViewModel(AuthenticationService authenticationService, HubService hubService)
    {
        this.authenticationService = authenticationService;
        HubService = hubService;

        OpenGroupCommandAsync = new AsyncRelayCommand<string?>(OpenGroupAsync);

        WeakReferenceMessenger.Default.Register<GroupsMessage>(this,
            async (_, _) => await UpdateGroupsAsync());
    }

    private async Task UpdateGroupsAsync()
    {
        await HubService.StartAsync();
        HubService.Group += async () =>
        {
            var request = (await authenticationService.GroupsAsync());
            if (request.IsSuccess)
            {
                Groups = request.Value;
            }
        };

        var request = (await authenticationService.GroupsAsync());
        if (request.IsSuccess)
        {
            Groups = request.Value;

            foreach (var group in Groups)
            {
                await HubService.JoinGroupAsync(group);
            }
        }
    }

    private async Task OpenGroupAsync(string? name)
    {
        await Shell.Current.GoToAsync($"{nameof(MessagesView)}?group={name}");
    }
}