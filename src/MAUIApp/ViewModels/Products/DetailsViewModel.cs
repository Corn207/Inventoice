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

public partial class DetailsViewModel(IProductService productService) : ObservableObject, IQueryAttributable
{
	public const string RouteName = "ProductDetails";
	public const string QueryId = "id";

	[ObservableProperty]
	private string? _id;

	[ObservableProperty]
	private ProductDetails? _model;

	[ObservableProperty]
	private bool _isRefreshing = false;

	public bool CanExecuteToEditPage => Model is not null;
	public bool CanExecuteDelete => Model is not null;


	[RelayCommand]
	private async Task LoadDataAsync()
	{
		if (Id is not null)
		{
			try
			{
				var entity = await productService.GetAsync(Id);
				Model = ProductMapper.ToDetails(entity);
			}
			catch (HttpServiceException)
			{
				StaticViewModel.ClosePageCommand.Execute(null);
				return;
			}

			ToEditPageCommand.NotifyCanExecuteChanged();
			DeleteCommand.NotifyCanExecuteChanged();
		}

		IsRefreshing = false;
	}

	[RelayCommand(CanExecute = nameof(CanExecuteToEditPage))]
	private async Task ToEditPageAsync()
	{
		var queries = new ShellNavigationQueryParameters()
		{
			{ CreateUpdateViewModel.QueryModel, Model! }
		};
		await NavigationService.ToAsync(CreateUpdateViewModel.RouteName, queries);
	}

	[RelayCommand(CanExecute = nameof(CanExecuteDelete))]
	private async Task DeleteAsync()
	{
		try
		{
			await productService.DeleteAsync(Id!);
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
		if (query.TryGetValue(QueryId, out var id))
		{
			var casted = (string)id;
			Id = casted;
			IsRefreshing = true;
		}
	}
}
