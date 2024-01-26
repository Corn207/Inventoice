using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Domain.DTOs.ExportReports;
using Domain.DTOs.Products;
using MAUIApp.Mappers;
using MAUIApp.Models.Products;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;
using MAUIApp.ViewModels.Messenger;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MAUIApp.ViewModels.ExportReports;

public partial class CreateViewModel : ObservableObject, IQueryAttributable
{
	public CreateViewModel(
		IExportReportService exportReportService,
		NavigationService navigationService)
	{
		_exportReportService = exportReportService;
		_navigationService = navigationService;
		Items.CollectionChanged += Items_CollectionChanged;
	}

	public const string RouteName = "ExportReportCreate";
	public const string QueryAddProduct = "addproduct";
	private readonly IExportReportService _exportReportService;
	private readonly NavigationService _navigationService;

	#region Properties
	public ObservableCollection<ProductItem> Items { get; } = [];
	public long TotalProduct => Items.Count;
	public long TotalItem => Items.Sum(x => x.Quantity);
	public bool CanExecuteSave => Items.Count > 0;
	#endregion


	[RelayCommand(CanExecute = nameof(CanExecuteSave))]
	private async Task SaveAsync()
	{
		var items = Items.Select(x => new ExportReportCreateProductItem(x.Id, x.Quantity)).ToArray();
		var create = new ExportReportCreate(items);
		try
		{
			await _exportReportService.CreateAsync(create);
		}
		catch (HttpServiceException)
		{
			return;
		}

		WeakReferenceMessenger.Default.Send(new ExportReportListRefreshMessage());
		await NavigationService.BackAsync();
	}

	[RelayCommand]
	private void ClearItems()
	{
		Items.Clear();
	}

	[RelayCommand]
	private void RemoveItem(ProductItem item)
	{
		Items.Remove(item);
	}

	[RelayCommand]
	private async Task OpenEditItemPopupAsync(ProductItem item)
	{
		if (item.InStock == 1) return;
		await _navigationService.ShowPopup<EditItemViewModel>(x =>
		{
			x.Item = item;
			x.ValuedChanged += (s, e) => NotifyTotalPropertiesChanged();
		});
	}


	private void NotifyTotalPropertiesChanged()
	{
		OnPropertyChanged(nameof(TotalProduct));
		OnPropertyChanged(nameof(TotalItem));
	}

	private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		NotifyTotalPropertiesChanged();
		SaveCommand.NotifyCanExecuteChanged();
	}

	public async void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.TryGetValue(QueryAddProduct, out var productShort))
		{
			var casted = (ProductShort)productShort;
			if (casted.InStock == 0)
			{
				await NavigationService.DisplayOutOfStockAlert();
			}
			else if (Items.Any(x => x.Id == casted.Id))
			{
				await NavigationService.DisplayDuplicateProductAlert();
			}
			else
			{
				var item = ProductMapper.ToItem(casted);
				item.Quantity = 1;
				Items.Add(item);
			}
		}
	}
}
