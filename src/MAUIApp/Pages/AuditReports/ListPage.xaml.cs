using MAUIApp.ViewModels.AuditReports;

namespace MAUIApp.Pages.AuditReports;

public partial class ListPage : ContentPage
{
	public ListPage(ListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
