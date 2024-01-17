using MAUIApp.ViewModels.Invoices;

namespace MAUIApp.Pages.Invoices;

public partial class ListPage : ContentPage
{
	public ListPage(ListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
