using MAUIApp.ViewModels.Users;

namespace MAUIApp.Pages.Users;

public partial class DetailsUserPage : ContentPage
{
	public DetailsUserPage(DetailsUserViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
