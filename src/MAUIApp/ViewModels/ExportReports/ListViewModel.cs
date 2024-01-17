using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
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
			TotalAllItems = await _exportReportService.TotalAsync();

			var shorts = await _exportReportService.SearchAsync(
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
