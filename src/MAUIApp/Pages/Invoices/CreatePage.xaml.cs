using MAUIApp.ViewModels.Invoices;

namespace MAUIApp.Pages.Invoices;

public partial class CreatePage : ContentPage
{
	public CreatePage(CreateViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
