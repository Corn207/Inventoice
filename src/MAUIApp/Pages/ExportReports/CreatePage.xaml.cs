using MAUIApp.ViewModels.ExportReports;

namespace MAUIApp.Pages.ExportReports;

public partial class CreatePage : ContentPage
{
	public CreatePage(CreateViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
