using MAUIApp.ViewModels;

namespace MAUIApp.Pages;

public partial class DebugPage : ContentPage
{
	public DebugPage(DebugViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
