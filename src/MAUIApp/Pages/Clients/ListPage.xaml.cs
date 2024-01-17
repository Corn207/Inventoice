using MAUIApp.ViewModels.Clients;

namespace MAUIApp.Pages.Clients;

public partial class ListPage : ContentPage
{
	public ListPage(ListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
