using MAUIApp.ViewModels.ExportReports;

namespace MAUIApp.Pages.ExportReports;

public partial class ListPage : ContentPage
{
	public ListPage(ListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
