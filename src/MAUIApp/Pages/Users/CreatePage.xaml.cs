using MAUIApp.ViewModels.Users;

namespace MAUIApp.Pages.Users;

public partial class CreatePage : ContentPage
{
    public CreatePage(CreateViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
