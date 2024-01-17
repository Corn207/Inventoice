using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Domain.DTOs.Clients;
using Domain.DTOs.Invoices;
using Domain.DTOs.Products;
using MAUIApp.Mappers;
using MAUIApp.Models.Products;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;
using MAUIApp.ViewModels.Messenger;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MAUIApp.ViewModels.Invoices;
public partial class CreateViewModel : ObservableObject, IQueryAttributable
{
	public CreateViewModel(
		IInvoiceService invoiceService,
		NavigationService navigationService)
	{
		_invoiceService = invoiceService;
		_navigationService = navigationService;
		Items.CollectionChanged += Items_CollectionChanged;
	}

	public const string RouteName = "InvoiceCreate";
	public const string QueryAddProduct = "addproduct";
	public const string QuerySetClient = "client";
	private readonly IInvoiceService _invoiceService;
	private readonly NavigationService _navigationService;

	#region Properties
	private ClientShort? _client;

	[ObservableProperty]
	private uint _grandTotal;

	[ObservableProperty]
	private bool _isPaid;

	public ObservableCollection<ProductItem> Items { get; } = [];
	public long TotalItem => Items.Sum(x => x.Quantity);
	public long TotalPrice => Items.Sum(x => x.Quantity * x.Price);
	public long TotalProduct => Items.Count;
	public string? ClientName => _client?.Name;
	public bool CanExecuteSave => Items.Count > 0;


	#endregion


	[RelayCommand(CanExecute = nameof(CanExecuteSave))]
	private async Task SaveAsync()
	{
		var items = Items
			.Select(x => new InvoiceCreateProductItem(x.Id, x.Quantity))
			.ToArray();
		var create = new InvoiceCreate()
		{
			ClientId = _client?.Id,
			ProductItems = items,
			GrandTotal = GrandTotal,
			IsPaid = IsPaid,
		};

		try
		{
			await _invoiceService.CreateAsync(create);
		}
		catch (HttpServiceException)
		{
			return;
		}

		WeakReferenceMessenger.Default.Send(new InvoiceListRefreshMessage());
		await NavigationService.BackAsync();
	}

	[RelayCommand]
	private void ClearItems()
	{
		Items.Clear();
	}

	[RelayCommand]
	private void RemoveItem(ProductItem item)
	{
		Items.Remove(item);
	}

	[RelayCommand]
	private async Task OpenEditItemPopupAsync(ProductItem item)
	{
		if (item.InStock <= 1) return;
		await _navigationService.ShowPopup<EditItemViewModel>(x =>
		{
			x.Item = item;
			x.ValuedChanged += (s, e) => NotifyTotalPropertiesChanged();
		});
	}


	private void NotifyTotalPropertiesChanged()
	{
		OnPropertyChanged(nameof(TotalItem));
		OnPropertyChanged(nameof(TotalPrice));
		OnPropertyChanged(nameof(TotalProduct));
		GrandTotal = Convert.ToUInt32(TotalPrice);
	}

	private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		NotifyTotalPropertiesChanged();
		SaveCommand.NotifyCanExecuteChanged();
	}

	public async void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.TryGetValue(QueryAddProduct, out var productShort))
		{
			var casted = (ProductShort)productShort;
			if (casted.InStock == 0)
			{
				await NavigationService.DisplayOutOfStockAlert();
			}
			else if (Items.Any(x => x.Id == casted.Id))
			{
				await NavigationService.DisplayDuplicateProductAlert();
			}
			else
			{
				var item = ProductMapper.ToItem(casted);
				item.Quantity = 1;
				Items.Add(item);
			}
		}

		if (query.TryGetValue(QuerySetClient, out var clientShort))
		{
			_client = (ClientShort)clientShort;
			OnPropertyChanged(nameof(ClientName));
		}
	}
}
