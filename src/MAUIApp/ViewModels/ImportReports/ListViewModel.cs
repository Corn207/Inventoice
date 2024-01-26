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

	private readonly Filter _filter = new();

	[ObservableProperty]
	private IEnumerable<GroupShort> _items = [];

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
			TotalAllItems = await _importReportService.TotalAsync();

			var shorts = await _importReportService.SearchAsync(
				_filter.ProductNameOrBarcode,
				_filter.AuthorName,
				_filter.DateStart,
				_filter.DateEnd,
				_filter.OrderBy);

			Items = shorts.GroupBy(
				x =>
				{
					var local = LocalizationService.ToLocalTime(x.DateCreated);
					var date = DateOnly.FromDateTime(local);
					return date;
				},
				(date, shorts) => new GroupShort(date, shorts.ToList()));
			TotalFoundItems = Convert.ToUInt32(shorts.Count());
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
