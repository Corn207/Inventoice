using MAUIApp.ViewModels.ImportReports;

namespace MAUIApp.Pages.ImportReports;

public partial class ListPage : ContentPage
{
	public ListPage(ListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
