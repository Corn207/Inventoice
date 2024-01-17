using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Domain.Entities;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;
using MAUIApp.ViewModels.Messenger;

namespace MAUIApp.ViewModels.ImportReports;

public partial class DetailsViewModel(IImportReportService importReportService) : ObservableObject, IQueryAttributable
{
	public const string RouteName = "ImportReportDetails";
	public const string QueryId = "id";

	[ObservableProperty]
	private string? _id;

	[ObservableProperty]
	private ImportReport? _model;

	[ObservableProperty]
	private bool _isRefreshing = false;

	public int? TotalProduct => Model?.ProductItems.Count;
	public long? TotalItem => Model?.ProductItems.Sum(x => x.Quantity);
	public long? TotalPrice => Model?.ProductItems.Sum(x => x.Quantity * x.Price);
	public bool CanExecuteCancel => Model is not null && !Model.DateCancelled.HasValue;
	public bool CanExecuteDelete => Model is not null;


	[RelayCommand]
	private async Task LoadDataAsync()
	{
		if (Id is not null)
		{
			try
			{
				Model = await importReportService.GetAsync(Id);
			}
			catch (HttpServiceException)
			{
				StaticViewModel.ClosePageCommand.Execute(null);
				return;
			}

			OnPropertyChanged(nameof(TotalProduct));
			OnPropertyChanged(nameof(TotalItem));
			OnPropertyChanged(nameof(TotalPrice));
			CancelCommand.NotifyCanExecuteChanged();
			DeleteCommand.NotifyCanExecuteChanged();
		}

		IsRefreshing = false;
	}

	[RelayCommand(CanExecute = nameof(CanExecuteCancel))]
	private async Task CancelAsync()
	{
		try
		{
			await importReportService.CancelAsync(Id!);
		}
		catch (HttpServiceException)
		{
			return;
		}

		IsRefreshing = true;
	}

	[RelayCommand(CanExecute = nameof(CanExecuteDelete))]
	private async Task DeleteAsync()
	{
		try
		{
			await importReportService.DeleteAsync(Id!);
		}
		catch (HttpServiceException)
		{
			return;
		}

		WeakReferenceMessenger.Default.Send(new ImportReportListRefreshMessage());
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
