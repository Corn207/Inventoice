using MAUIApp.ViewModels.Products;

namespace MAUIApp.Pages.Products;

public partial class BarcodeScannerPage : ContentPage
{
	public BarcodeScannerPage(BarcodeScannerViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
