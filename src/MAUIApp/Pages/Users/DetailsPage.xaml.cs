using MAUIApp.ViewModels.Users;

namespace MAUIApp.Pages.Users;
public partial class DetailsPage : ContentPage
{
	public DetailsPage(DetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
