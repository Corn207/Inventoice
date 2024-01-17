using MAUIApp.ViewModels.AuditReports;

namespace MAUIApp.Pages.AuditReports;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(DetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
