using MAUIApp.ViewModels.Users;

namespace MAUIApp.Pages.Users;

public partial class FilterPage : ContentPage
{
	public FilterPage(FilterViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
