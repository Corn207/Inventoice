using MAUIApp.ViewModels.Users;

namespace MAUIApp.Pages.Users;

public partial class EditMePage : ContentPage
{
	public EditMePage(EditMeViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
