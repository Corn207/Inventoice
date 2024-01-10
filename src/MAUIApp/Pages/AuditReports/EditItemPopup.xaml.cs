using CommunityToolkit.Maui.Views;
using MAUIApp.ViewModels.AuditReports;

namespace MAUIApp.Pages.AuditReports;

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
