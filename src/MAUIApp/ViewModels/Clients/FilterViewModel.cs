using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MAUIApp.Models.Clients;
using MAUIApp.Services;
using MAUIApp.ViewModels.Messenger;

namespace MAUIApp.ViewModels.Clients;

public partial class FilterViewModel : ObservableObject, IQueryAttributable
{
	public const string RouteName = "ClientFilter";
	public const string QueryFilter = "filter";

	[ObservableProperty]
	private Filter _filter = new();


	[RelayCommand]
	private static async Task SearchAsync()
	{
		WeakReferenceMessenger.Default.Send(new ClientListRefreshMessage());
		await NavigationService.BackAsync();
	}

	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.TryGetValue(QueryFilter, out var filter))
		{
			var casted = (Filter)filter;
			Filter = casted;
		}
	}
}
