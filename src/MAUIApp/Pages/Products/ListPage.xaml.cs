using MAUIApp.ViewModels.Products;

namespace MAUIApp.Pages.Products;

public partial class ListPage : ContentPage
{
	public ListPage(ListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
