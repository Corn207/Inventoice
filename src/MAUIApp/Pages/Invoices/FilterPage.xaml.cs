using MAUIApp.ViewModels.Invoices;

namespace MAUIApp.Pages.Invoices;

public partial class FilterPage : ContentPage
{
	public FilterPage(FilterViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
