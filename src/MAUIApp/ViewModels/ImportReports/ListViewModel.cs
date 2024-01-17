using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Domain.DTOs.ImportReports;
using MAUIApp.Mappers;
using MAUIApp.Models;
using MAUIApp.Models.ImportReports;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;
using MAUIApp.ViewModels.Messenger;

namespace MAUIApp.ViewModels.ImportReports;

public partial class ListViewModel : ObservableRecipient, IRecipient<ImportReportListRefreshMessage>, IQueryAttributable
{
	public ListViewModel(IImportReportService importReportService)
	{
		IsActive = true;
		_importReportService = importReportService;
	}

	public const string RouteName = "ImportReportList";
	public const string QueryRefresh = "refresh";
	private readonly IImportReportService _importReportService;

	public Filter Filter { get; private set; } = new Filter();

	[ObservableProperty]
	private IEnumerable<GroupingByDate<ImportReportShort>> _items = [];

	[ObservableProperty]
	private bool _isRefreshing = true;

	[ObservableProperty]
	private uint _totalFoundItems;

	[ObservableProperty]
	private uint _totalAllItems;


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
			var shorts = await _importReportService.SearchAsync(
				Filter.ProductNameOrBarcode,
				Filter.AuthorName,
				Filter.DateStart,
				Filter.DateEnd,
				Filter.OrderBy);
			Items = Mapper.ToGroupingByDateCreated(shorts);
			TotalAllItems = await _importReportService.CountAllAsync();
			TotalFoundItems = await _importReportService.CountAsync(
				Filter.ProductNameOrBarcode,
				Filter.AuthorName,
				Filter.DateStart,
				Filter.DateEnd);
		}
		catch (HttpServiceException)
		{
		}
		IsRefreshing = false;
	}


	public void Receive(ImportReportListRefreshMessage message)
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
