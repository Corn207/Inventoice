using MAUIApp.ViewModels.Products;

namespace MAUIApp.Pages.Products;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(DetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
