using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Domain.DTOs.Users;
using MAUIApp.Models.Users;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;
using MAUIApp.ViewModels.Messenger;

namespace MAUIApp.ViewModels.Users;

public partial class ListViewModel : ObservableRecipient, IRecipient<UserListRefreshMessage>, IQueryAttributable
{
	public ListViewModel(IUserService service)
	{
		IsActive = true;
		_service = service;
	}

	public const string RouteName = "UserList";
	public const string QueryRefresh = "refresh";
	private readonly IUserService _service;

	private readonly Filter _filter = new();

	[ObservableProperty]
	private IEnumerable<UserShort> _items = [];

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
		await NavigationService.ToAsync(CreateViewModel.RouteName);
	}

	[RelayCommand]
	private async Task LoadDataAsync()
	{
		try
		{
			TotalAllItems = await _service.TotalAsync();

			var shorts = await _service.SearchAsync(_filter.Name, _filter.OrderBy);
			Items = shorts;
			TotalFoundItems = Convert.ToUInt32(shorts.Count());
		}
		catch (HttpServiceException)
		{
		}

		IsRefreshing = false;
	}


	public void Receive(UserListRefreshMessage message)
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
