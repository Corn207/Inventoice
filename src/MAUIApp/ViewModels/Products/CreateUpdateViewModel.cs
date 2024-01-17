using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MAUIApp.Mappers;
using MAUIApp.Models.Products;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;
using MAUIApp.ViewModels.Messenger;

namespace MAUIApp.ViewModels.Products;

public partial class CreateUpdateViewModel(IProductService productService) : ObservableObject, IQueryAttributable
{
	public const string RouteName = "ProductCreateUpdate";
	public const string QueryModel = "model";
	public const string QueryProductNameOrBarcode = "product";

	[ObservableProperty]
	private ProductDetails _model = new();


	[RelayCommand]
	private async Task SaveAsync()
	{
		var create = ProductMapper.ToCreateUpdate(Model);
		try
		{
			if (string.IsNullOrWhiteSpace(Model.Id))
			{
				await productService.CreateAsync(create);
			}
			else
			{
				await productService.UpdateAsync(Model.Id, create);
			}
		}
		catch (HttpServiceException)
		{
			return;
		}

		WeakReferenceMessenger.Default.Send(new ProductListRefreshMessage());
		await NavigationService.BackAsync();
	}


	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.TryGetValue(QueryModel, out var model))
		{
			var casted = (ProductDetails)model;
			Model = casted;
		}
		
		if (query.TryGetValue(QueryProductNameOrBarcode, out var product))
		{
			var casted = (string)product;
			Model.Barcode = casted;
		}
	}
}
