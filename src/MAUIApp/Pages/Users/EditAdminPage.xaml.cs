using MAUIApp.ViewModels.Users;

namespace MAUIApp.Pages.Users;

public partial class EditAdminPage : ContentPage
{
	public EditAdminPage(EditAdminViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
