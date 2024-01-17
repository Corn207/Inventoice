using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Domain.DTOs.Products;
using MAUIApp.Models.Products;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;
using MAUIApp.ViewModels.Messenger;

namespace MAUIApp.ViewModels.Products;

public partial class ListViewModel : ObservableRecipient, IRecipient<ProductListRefreshMessage>, IQueryAttributable
{
	public ListViewModel(IProductService productService)
	{
		IsActive = true;
		_productService = productService;
	}

	public const string RouteName = "ProductList";
	public const string QueryRefresh = "refresh";
	private readonly IProductService _productService;

	private readonly Filter _filter = new();

	[ObservableProperty]
	private IEnumerable<ProductShort> _items = [];

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
			TotalAllItems = await _productService.TotalAsync();

			var shorts = await _productService.SearchAsync(_filter.ProductNameOrBarcode, _filter.OrderBy);
			Items = shorts;
			TotalFoundItems = Convert.ToUInt32(shorts.Count());
		}
		catch (HttpServiceException)
		{
		}

		IsRefreshing = false;
	}


	public void Receive(ProductListRefreshMessage message)
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
