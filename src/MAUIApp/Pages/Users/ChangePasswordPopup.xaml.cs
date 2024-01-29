using CommunityToolkit.Maui.Views;
using MAUIApp.ViewModels.Users;

namespace MAUIApp.Pages.Users;

public partial class ChangePasswordPopup : Popup
{
	public ChangePasswordPopup(ChangePasswordViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		viewModel.OnChangePasswordSuccess += (s, e) => Close();
	}
}
