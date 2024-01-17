using CommunityToolkit.Maui.Views;
using MAUIApp.ViewModels.ExportReports;

namespace MAUIApp.Pages.ExportReports;

public partial class EditItemPopup : Popup
{
	public EditItemPopup(EditItemViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}

	private void OKButton_Clicked(object sender, EventArgs e)
	{
		Close();
	}
}
