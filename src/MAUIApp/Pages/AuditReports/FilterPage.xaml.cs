using MAUIApp.ViewModels.AuditReports;

namespace MAUIApp.Pages.AuditReports;

public partial class FilterPage : ContentPage
{
	public FilterPage(FilterViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
