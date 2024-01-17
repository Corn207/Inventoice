using MAUIApp.ViewModels.Products;

namespace MAUIApp.Pages.Products;

public partial class CreateUpdatePage : ContentPage
{
    public CreateUpdatePage(CreateUpdateViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
