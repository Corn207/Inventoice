using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Domain.DTOs.Products;
using MAUIApp.Mappers;
using MAUIApp.Models.Products;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;
using MAUIApp.ViewModels.Messenger;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using static Android.Graphics.ColorSpace;

namespace MAUIApp.ViewModels.AuditReports;

public partial class CreateViewModel : ObservableObject, IQueryAttributable
{
	public CreateViewModel(
		ICredentialService credentialService,
		IAuditReportService auditReportService,
		NavigationService navigationService)
	{
		_credentialService = credentialService;
		_auditReportService = auditReportService;
		_navigationService = navigationService;
		Items.CollectionChanged += Items_CollectionChanged;
	}

	public const string RouteName = "AuditReportCreate";
	public const string QueryAddProduct = "addproduct";
	private readonly ICredentialService _credentialService;
	private readonly IAuditReportService _auditReportService;
	private readonly NavigationService _navigationService;

	#region Properties
	public ObservableCollection<ProductItem> Items { get; } = [];
	public long TotalProduct => Items.Count;
	public long TotalOriginal => Items.Sum(x => x.InStock);
	public long TotalAdjusted => Items.Sum(x => x.Quantity);
	public bool CanExecuteSave => Items.Count > 0;
	#endregion


	[RelayCommand(CanExecute = nameof(CanExecuteSave))]
	private async Task SaveAsync()
	{
		var create = AuditReportMapper.ToCreate(_credentialService.User.Id!, Items);
		try
		{
			await _auditReportService.CreateAsync(create);
		}
		catch (HttpServiceException)
		{
			return;
		}

		WeakReferenceMessenger.Default.Send(new AuditReportListRefreshMessage());
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
		OnPropertyChanged(nameof(TotalProduct));
		OnPropertyChanged(nameof(TotalOriginal));
		OnPropertyChanged(nameof(TotalAdjusted));
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
				item.Quantity = item.InStock;
				Items.Add(item);
			}
		}
	}
}
