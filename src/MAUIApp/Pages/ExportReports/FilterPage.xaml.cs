using MAUIApp.ViewModels.ExportReports;

namespace MAUIApp.Pages.ExportReports;

public partial class FilterPage : ContentPage
{
	public FilterPage(FilterViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
