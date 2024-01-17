using MAUIApp.ViewModels.Clients;

namespace MAUIApp.Pages.Clients;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(DetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
