using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUIApp.Models.Products;

namespace MAUIApp.ViewModels.Invoices;

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
