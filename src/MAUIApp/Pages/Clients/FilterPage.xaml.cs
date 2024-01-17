using MAUIApp.ViewModels.Clients;

namespace MAUIApp.Pages.Clients;

public partial class FilterPage : ContentPage
{
	public FilterPage(FilterViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
