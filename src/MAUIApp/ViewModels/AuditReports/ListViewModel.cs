using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Domain.DTOs.AuditReports;
using MAUIApp.Mappers;
using MAUIApp.Models;
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

	public Filter Filter { get; private set; } = new Filter();

	[ObservableProperty]
	private IEnumerable<GroupingByDate<AuditReportShort>> _items = [];

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
			var shorts = await _auditReportService.SearchAsync(
				Filter.ProductNameOrBarcode,
				Filter.AuthorName,
				Filter.DateStart,
				Filter.DateEnd,
				Filter.OrderBy);
			Items = Mapper.ToGroupingByDateCreated(shorts);
			TotalAllItems = await _auditReportService.CountAllAsync();
			TotalFoundItems = await _auditReportService.CountAsync(
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
