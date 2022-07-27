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

    private readonly AuthenticationService authenticationService;

    public GroupsViewModel(AuthenticationService authenticationService)
    {
        this.authenticationService = authenticationService;

        WeakReferenceMessenger.Default.Register<GroupsMessage>(this,
            async (_, _) => await UpdateGroupsAsync());
    }

    private async Task UpdateGroupsAsync()
    {
        Groups = (await authenticationService.GroupsAsync()).Value;
    }
}