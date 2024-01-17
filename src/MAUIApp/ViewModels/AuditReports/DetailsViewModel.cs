using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Domain.Entities;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;
using MAUIApp.ViewModels.Messenger;

namespace MAUIApp.ViewModels.AuditReports;

public partial class DetailsViewModel(IAuditReportService auditReportService) : ObservableObject, IQueryAttributable
{
	public const string RouteName = "AuditReportDetails";
	public const string QueryId = "id";

	[ObservableProperty]
	private string? _id;

	[ObservableProperty]
	private AuditReport? _model;

	[ObservableProperty]
	private bool _isRefreshing = false;

	public long? TotalProduct => Model?.ProductItems.Count;
	public long? TotalOriginal => Model?.ProductItems.Sum(x => x.OriginalQuantity);
	public long? TotalAdjusted => Model?.ProductItems.Sum(x => x.AdjustedQuantity);
	public bool CanExecuteDelete => Model is not null;


	[RelayCommand]
	private async Task LoadDataAsync()
	{
		if (Id is not null)
		{
			try
			{
				Model = await auditReportService.GetAsync(Id);
			}
			catch (HttpServiceException)
			{
				StaticViewModel.ClosePageCommand.Execute(null);
				return;
			}

			OnPropertyChanged(nameof(TotalProduct));
			OnPropertyChanged(nameof(TotalOriginal));
			OnPropertyChanged(nameof(TotalAdjusted));
			DeleteCommand.NotifyCanExecuteChanged();
		}

		IsRefreshing = false;
	}

	[RelayCommand(CanExecute = nameof(CanExecuteDelete))]
	private async Task DeleteAsync()
	{
		try
		{
			await auditReportService.DeleteAsync(Id!);
		}
		catch (HttpServiceException)
		{
			return;
		}

		WeakReferenceMessenger.Default.Send(new AuditReportListRefreshMessage());
		await NavigationService.BackAsync();
	}


	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.TryGetValue(QueryId, out var id))
		{
			var casted = (string)id;
			Id = casted;
			IsRefreshing = true;
		}
	}
}
