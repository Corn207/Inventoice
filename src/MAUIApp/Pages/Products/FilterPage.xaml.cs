using MAUIApp.ViewModels.Products;

namespace MAUIApp.Pages.Products;

public partial class FilterPage : ContentPage
{
	public FilterPage(FilterViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
