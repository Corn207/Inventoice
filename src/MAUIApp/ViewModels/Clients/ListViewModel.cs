using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Domain.DTOs.Clients;
using MAUIApp.Models.Clients;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;
using MAUIApp.ViewModels.Messenger;

namespace MAUIApp.ViewModels.Clients;

public partial class ListViewModel : ObservableRecipient, IRecipient<ClientListRefreshMessage>, IQueryAttributable
{
	public ListViewModel(IClientService clientService)
	{
		IsActive = true;
		_clientService = clientService;
	}

	public const string RouteName = "ClientList";
	public const string QueryRefresh = "refresh";
	private readonly IClientService _clientService;

	private readonly Filter _filter = new();

	[ObservableProperty]
	private IEnumerable<ClientShort> _items = [];

	[ObservableProperty]
	private bool _isRefreshing = true;

	[ObservableProperty]
	private uint? _totalFoundItems;

	[ObservableProperty]
	private uint? _totalAllItems;


	[RelayCommand]
	private async Task ToFilterPageAsync()
	{
		var queries = new ShellNavigationQueryParameters()
		{
			{ FilterViewModel.QueryFilter, _filter },
		};
		await NavigationService.ToAsync(FilterViewModel.RouteName, queries);
	}

	[RelayCommand]
	private static async Task ToCreatePageAsync()
	{
		await NavigationService.ToAsync(CreateUpdateViewModel.RouteName);
	}

	[RelayCommand]
	private async Task LoadDataAsync()
	{
		try
		{
			TotalAllItems = await _clientService.TotalAsync();

			var shorts = await _clientService.SearchAsync(_filter.NameOrPhonenumber, _filter.OrderBy);
			Items = shorts;
			TotalFoundItems = Convert.ToUInt32(shorts.Count());
		}
		catch (HttpServiceException)
		{
		}

		IsRefreshing = false;
	}


	public void Receive(ClientListRefreshMessage message)
	{
		IsRefreshing = true;
	}

	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.ContainsKey(QueryRefresh))
		{
			IsRefreshing = true;
		}
	}
}
