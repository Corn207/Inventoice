using MAUIApp.ViewModels.ImportReports;

namespace MAUIApp.Pages.ImportReports;

public partial class CreatePage : ContentPage
{
	public CreatePage(CreateViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
