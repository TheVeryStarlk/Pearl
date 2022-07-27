using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Pearl.Maui.Messages;
using Pearl.Maui.Services;

namespace Pearl.Maui.ViewModels;

public sealed class GroupsViewModel : ObservableObject
{
    public string[]? Groups
    {
        get => groups;
        set => SetProperty(ref groups, value);
    }

    private string[]? groups;

    public HubService HubService { get; }

    private readonly AuthenticationService authenticationService;

    public GroupsViewModel(AuthenticationService authenticationService, HubService hubService)
    {
        this.authenticationService = authenticationService;
        HubService = hubService;

        WeakReferenceMessenger.Default.Register<GroupsMessage>(this,
            async (_, _) => await UpdateGroupsAsync());
    }

    private async Task UpdateGroupsAsync()
    {
        Groups = (await authenticationService.GroupsAsync()).Value;

        await HubService.StartAsync();
        HubService.Group += async () => Groups = (await authenticationService.GroupsAsync()).Value;
    }
}