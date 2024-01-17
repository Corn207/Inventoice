using MAUIApp.ViewModels.ExportReports;

namespace MAUIApp.Pages.ExportReports;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(DetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
