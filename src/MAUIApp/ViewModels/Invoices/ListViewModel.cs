using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Domain.DTOs.Invoices;
using MAUIApp.Mappers;
using MAUIApp.Models;
using MAUIApp.Models.Invoices;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;
using MAUIApp.ViewModels.Messenger;

namespace MAUIApp.ViewModels.Invoices;

public partial class ListViewModel : ObservableRecipient, IRecipient<InvoiceListRefreshMessage>, IQueryAttributable
{
	public ListViewModel(IInvoiceService invoiceService)
	{
		IsActive = true;
		_invoiceService = invoiceService;
	}

	public const string RouteName = "InvoiceList";
	public const string QueryRefresh = "refresh";
	private readonly IInvoiceService _invoiceService;

	public Filter Filter { get; private set; } = new Filter();

	[ObservableProperty]
	private IEnumerable<GroupingByDate<InvoiceShort>> _items = [];

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
		await NavigationService.ToAsync(CreateViewModel.RouteName);
	}

	[RelayCommand]
	private async Task LoadDataAsync()
	{
		try
		{
			var shorts = await _invoiceService.SearchAsync(
				Filter.ProductNameOrBarcode,
				Filter.ClientNameOrPhonenumber,
				Filter.AuthorName,
				Filter.Status,
				Filter.DateStart,
				Filter.DateEnd,
				Filter.OrderBy);
			Items = Mapper.ToGroupingByDateCreated(shorts);
			TotalAllItems = await _invoiceService.CountAllAsync();
			TotalFoundItems = await _invoiceService.CountAsync(
				Filter.ProductNameOrBarcode,
				Filter.ClientNameOrPhonenumber,
				Filter.AuthorName,
				Filter.Status,
				Filter.DateStart,
				Filter.DateEnd);
		}
		catch (HttpServiceException)
		{
		}

		IsRefreshing = false;
	}


	public void Receive(InvoiceListRefreshMessage message)
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
