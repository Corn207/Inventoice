using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Domain.DTOs.ImportReports;
using Domain.DTOs.Products;
using MAUIApp.Mappers;
using MAUIApp.Models.Products;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;
using MAUIApp.ViewModels.Messenger;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MAUIApp.ViewModels.ImportReports;

public partial class CreateViewModel : ObservableObject, IQueryAttributable
{
	public CreateViewModel(
		IImportReportService importReportService,
		NavigationService navigationService)
	{
		_importReportService = importReportService;
		_navigationService = navigationService;
		Items.CollectionChanged += Items_CollectionChanged;
	}

	public const string RouteName = "ImportReportCreate";
	public const string QueryAddProduct = "addproduct";
	private readonly IImportReportService _importReportService;
	private readonly NavigationService _navigationService;

	#region Properties
	public ObservableCollection<ProductItem> Items { get; } = [];
	public long? TotalItem => Items.Sum(x => x.Quantity);
	public long? TotalPrice => Items.Sum(x => x.Quantity * x.Price);
	public int? TotalProduct => Items.Count;
	public bool CanExecuteSave => Items.Count > 0;
	#endregion


	[RelayCommand(CanExecute = nameof(CanExecuteSave))]
	private async Task SaveAsync()
	{
		var items = Items.Select(x => new ImportReportCreateProductItem(x.Id, x.Price, x.Quantity)).ToArray();
		var create = new ImportReportCreate(items);
		try
		{
			await _importReportService.CreateAsync(create);
		}
		catch (HttpServiceException)
		{
			return;
		}

		WeakReferenceMessenger.Default.Send(new ImportReportListRefreshMessage());
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
		await _navigationService.ShowPopup<EditItemViewModel>(x =>
		{
			x.Item = item;
			x.ValuedChanged += (s, e) => NotifyTotalPropertiesChanged();
		});
	}


	private void NotifyTotalPropertiesChanged()
	{
		OnPropertyChanged(nameof(TotalItem));
		OnPropertyChanged(nameof(TotalPrice));
		OnPropertyChanged(nameof(TotalProduct));
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
			if (Items.Any(x => x.Id == casted.Id))
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
