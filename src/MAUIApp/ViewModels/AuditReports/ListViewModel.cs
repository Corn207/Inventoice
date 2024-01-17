using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MAUIApp.Models.AuditReports;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;
using MAUIApp.ViewModels.Messenger;

namespace MAUIApp.ViewModels.AuditReports;

public partial class ListViewModel : ObservableRecipient, IRecipient<AuditReportListRefreshMessage>, IQueryAttributable
{
	public ListViewModel(IAuditReportService auditReportService)
	{
		IsActive = true;
		_auditReportService = auditReportService;
	}

	public const string RouteName = "AuditReportList";
	public const string QueryRefresh = "refresh";
	private readonly IAuditReportService _auditReportService;

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
			TotalAllItems = await _auditReportService.TotalAsync();

			var shorts = await _auditReportService.SearchAsync(
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


	public void Receive(AuditReportListRefreshMessage message)
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
