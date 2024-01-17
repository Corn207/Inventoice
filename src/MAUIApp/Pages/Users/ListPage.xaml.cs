using MAUIApp.ViewModels.Users;

namespace MAUIApp.Pages.Users;
public partial class ListPage : ContentPage
{
	public ListPage(ListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
