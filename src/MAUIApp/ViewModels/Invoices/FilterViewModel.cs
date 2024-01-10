using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MAUIApp.Models.Invoices;
using MAUIApp.Services;
using MAUIApp.ViewModels.Messenger;

namespace MAUIApp.ViewModels.Invoices;

public partial class FilterViewModel : ObservableObject, IQueryAttributable
{
	public const string RouteName = "InvoiceFilter";
	public const string QueryFilter = "filter";
	public const string QueryProductSearch = "product";

	[ObservableProperty]
	private Filter _filter = new();


	[RelayCommand]
	private static async Task SearchAsync()
	{
		WeakReferenceMessenger.Default.Send(new InvoiceListRefreshMessage());
		await NavigationService.BackAsync();
	}

	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.TryGetValue(QueryFilter, out var filter))
		{
			var casted = (Filter)filter;
			Filter = casted;
		}

		if (query.TryGetValue(QueryProductSearch, out var product))
		{
			var casted = (string)product;
			Filter.ProductNameOrBarcode = casted;
		}
	}
}
