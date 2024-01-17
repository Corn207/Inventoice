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

	public Filter Filter { get; private set; } = new Filter();

	[ObservableProperty]
	private ClientShort[] _items = [];

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
			{ FilterViewModel.QueryFilter, Filter },
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
			Items = await _clientService.SearchAsync(Filter.NameOrPhonenumber, Filter.OrderBy);
			TotalAllItems = await _clientService.CountAllAsync();
			TotalFoundItems = await _clientService.CountAsync(Filter.NameOrPhonenumber);
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
