using MAUIApp.ViewModels.Invoices;

namespace MAUIApp.Pages.Invoices;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(DetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
