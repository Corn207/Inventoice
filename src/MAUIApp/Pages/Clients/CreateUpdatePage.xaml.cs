using MAUIApp.ViewModels.Clients;

namespace MAUIApp.Pages.Clients;

public partial class CreateUpdatePage : ContentPage
{
    public CreateUpdatePage(CreateUpdateViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
