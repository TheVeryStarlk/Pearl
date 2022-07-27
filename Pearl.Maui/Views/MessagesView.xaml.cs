using Pearl.Maui.ViewModels;

namespace Pearl.Maui.Views;

public partial class MessagesView : ContentPage
{
	public MessagesView(MessagesViewModel messagesViewModel)
	{
        BindingContext = messagesViewModel;
		InitializeComponent();
	}
}