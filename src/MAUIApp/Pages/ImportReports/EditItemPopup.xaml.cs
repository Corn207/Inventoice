using CommunityToolkit.Maui.Views;
using MAUIApp.ViewModels.ImportReports;

namespace MAUIApp.Pages.ImportReports;

public partial class EditItemPopup : Popup
{
	public EditItemPopup(EditItemViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	private void OKButton_Clicked(object sender, EventArgs e)
	{
		Close();
	}
}
