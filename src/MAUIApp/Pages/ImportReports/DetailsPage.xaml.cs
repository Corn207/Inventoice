using MAUIApp.ViewModels.ImportReports;

namespace MAUIApp.Pages.ImportReports;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(DetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
