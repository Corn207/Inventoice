using MAUIApp.ViewModels.Products;

namespace MAUIApp.Pages.Products;

public partial class SearchPage : ContentPage
{
	public SearchPage(SearchViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
