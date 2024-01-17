using MAUIApp.ViewModels.Clients;

namespace MAUIApp.Pages.Clients;

public partial class SearchPage : ContentPage
{
	public SearchPage(SearchViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
