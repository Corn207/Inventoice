using MAUIApp.ViewModels;

namespace MAUIApp.Pages;

public partial class MorePage : ContentPage
{
	public MorePage(MoreViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
