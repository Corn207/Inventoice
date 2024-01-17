using MAUIApp.ViewModels.AuditReports;

namespace MAUIApp.Pages.AuditReports;

public partial class CreatePage : ContentPage
{
	public CreatePage(CreateViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
