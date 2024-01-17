using MAUIApp.ViewModels.ImportReports;

namespace MAUIApp.Pages.ImportReports;

public partial class FilterPage : ContentPage
{
	public FilterPage(FilterViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}
