using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Pearl.Maui.Messages;
using Pearl.Maui.Services;
using Pearl.Maui.Views;

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
        async Task UpdateAsync()
        {
            var request = (await authenticationService.GroupsAsync());
            if (request.IsSuccess)
            {
                Groups = request.Value;
            }
        }

        await UpdateAsync();
        await HubService.StartAsync();
        HubService.Group += async () => await UpdateAsync();
    }

    private async Task OpenGroupAsync(string? name)
    {
        WeakReferenceMessenger.Default.Send(new MessagesRequest(name));
        await Shell.Current.GoToAsync($"{nameof(MessagesView)}?group={name}");
    }
}