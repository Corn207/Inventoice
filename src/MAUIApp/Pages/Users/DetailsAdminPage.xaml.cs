using MAUIApp.ViewModels.Users;

namespace MAUIApp.Pages.Users;

public partial class DetailsAdminPage : ContentPage
{
	public DetailsAdminPage(DetailsAdminViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
