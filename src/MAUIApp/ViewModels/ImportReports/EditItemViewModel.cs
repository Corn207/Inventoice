using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUIApp.Models.Products;

namespace MAUIApp.ViewModels.ImportReports;

public partial class EditItemViewModel : ObservableObject
{
	public event EventHandler? ValuedChanged;

	[ObservableProperty]
	private ProductItem? _item;

	[RelayCommand]
	private void NotifyChanged()
	{
		ValuedChanged?.Invoke(this, EventArgs.Empty);
	}
}
