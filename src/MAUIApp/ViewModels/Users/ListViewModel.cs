using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Domain.DTOs.Products;
using MAUIApp.Models.Products;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;
using MAUIApp.ViewModels.Messenger;

namespace MAUIApp.ViewModels.Users;

public partial class ListViewModel : ObservableRecipient, IRecipient<UserListRefreshMessage>, IQueryAttributable
{
	public ListViewModel(IProductService productService)
	{
		IsActive = true;
		_productService = productService;
	}

	public const string RouteName = "ProductList";
	public const string QueryRefresh = "refresh";
	private readonly IProductService _productService;

	public Filter Filter { get; private set; } = new Filter();

	[ObservableProperty]
	private ProductShort[] _items = [];

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
			Items = await _productService.SearchAsync(Filter.ProductNameOrBarcode, Filter.OrderBy);
			TotalAllItems = await _productService.CountAllAsync();
			TotalFoundItems = await _productService.CountAsync(Filter.ProductNameOrBarcode);
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
