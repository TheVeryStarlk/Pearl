using Pearl.Maui.ViewModels;

namespace Pearl.Maui.Views;

public sealed partial class GroupsView : ContentPage
{
	public GroupsView(GroupsViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}