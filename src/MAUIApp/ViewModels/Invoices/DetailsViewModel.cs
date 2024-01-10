using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Domain.Entities;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;
using MAUIApp.ViewModels.Messenger;

namespace MAUIApp.ViewModels.Invoices;

public partial class DetailsViewModel(IInvoiceService invoiceService) : ObservableObject, IQueryAttributable
{
	public const string RouteName = "InvoiceDetails";
	public const string QueryId = "id";

	[ObservableProperty]
	private string? _id;

	[ObservableProperty]
	private Invoice? _model;

	[ObservableProperty]
	private bool _isRefreshing = false;

	public int? TotalProduct => Model?.ProductItems.Count;
	public long? TotalItem => Model?.ProductItems.Sum(x => x.Quantity);
	public long? TotalPrice => Model?.ProductItems.Sum(x => x.Quantity * x.Price);
	public long? Discount => Model is null ? null : TotalPrice < Model.GrandTotal ? 0 : TotalPrice - Model.GrandTotal;
	public float? DiscountPercentage => (Model is null || Discount is null || TotalPrice is null) ? null :
		Discount <= 0 ? 0 :
		Discount == TotalPrice ? 100 :
		MathF.Floor((float)Discount / (float)TotalPrice * 100);
	public bool CanExecutePay => Model is not null && !Model.DateCancelled.HasValue && !Model.DatePaid.HasValue;
	public bool CanExecuteCancel => Model is not null && !Model.DateCancelled.HasValue;
	public bool CanExecuteDelete => Model is not null;


	[RelayCommand]
	private async Task LoadDataAsync()
	{
		if (Id is not null)
		{
			try
			{
				Model = await invoiceService.GetAsync(Id);
			}
			catch (HttpServiceException)
			{
				StaticViewModel.ClosePageCommand.Execute(null);
				return;
			}

			OnPropertyChanged(nameof(TotalProduct));
			OnPropertyChanged(nameof(TotalItem));
			OnPropertyChanged(nameof(TotalPrice));
			OnPropertyChanged(nameof(Discount));
			OnPropertyChanged(nameof(DiscountPercentage));
			PayCommand.NotifyCanExecuteChanged();
			CancelCommand.NotifyCanExecuteChanged();
			DeleteCommand.NotifyCanExecuteChanged();
		}

		IsRefreshing = false;
	}

	[RelayCommand(CanExecute = nameof(CanExecutePay))]
	private async Task PayAsync()
	{
		try
		{
			await invoiceService.PayAsync(Id!);
		}
		catch (HttpServiceException)
		{
			return;
		}

		IsRefreshing = true;
	}

	[RelayCommand(CanExecute = nameof(CanExecuteCancel))]
	private async Task CancelAsync()
	{
		try
		{
			await invoiceService.CancelAsync(Id!);
		}
		catch (HttpServiceException)
		{
			return;
		}

		IsRefreshing = true;
		WeakReferenceMessenger.Default.Send(new InvoiceListRefreshMessage());
	}

	[RelayCommand(CanExecute = nameof(CanExecuteDelete))]
	private async Task DeleteAsync()
	{
		try
		{
			await invoiceService.DeleteAsync(Id!);
		}
		catch (HttpServiceException)
		{
			return;
		}

		WeakReferenceMessenger.Default.Send(new InvoiceListRefreshMessage());
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
