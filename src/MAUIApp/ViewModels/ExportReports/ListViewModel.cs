using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Domain.DTOs.ExportReports;
using MAUIApp.Mappers;
using MAUIApp.Models;
using MAUIApp.Models.ExportReports;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;
using MAUIApp.ViewModels.Messenger;

namespace MAUIApp.ViewModels.ExportReports;

public partial class ListViewModel : ObservableRecipient, IRecipient<ExportReportListRefreshMessage>, IQueryAttributable
{
	public ListViewModel(IExportReportService exportReportService)
	{
		IsActive = true;
		_exportReportService = exportReportService;
	}

	public const string RouteName = "ExportReportList";
	public const string QueryRefresh = "refresh";
	private readonly IExportReportService _exportReportService;

	public Filter Filter { get; private set; } = new Filter();

	[ObservableProperty]
	private IEnumerable<GroupingByDate<ExportReportShort>> _items = [];

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
			var shorts = await _exportReportService.SearchAsync(
				Filter.ProductNameOrBarcode,
				Filter.AuthorName,
				Filter.DateStart,
				Filter.DateEnd,
				Filter.OrderBy);
			Items = Mapper.ToGroupingByDateCreated(shorts);
			TotalAllItems = await _exportReportService.CountAllAsync();
			TotalFoundItems = await _exportReportService.CountAsync(
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


	public void Receive(ExportReportListRefreshMessage message)
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
