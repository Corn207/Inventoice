using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.DTOs.Products;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;

namespace MAUIApp.ViewModels.Products;

public partial class SearchViewModel(IProductService productService) : ObservableObject, IQueryAttributable
{
	public const string RouteName = "ProductSearch";
	public const string QueryReturnId = "returnId";
	public const string QuerySearchTerm = "search";

	public string? ReturnId { get; set; }

	[ObservableProperty]
	private string _searchTerm = string.Empty;

	[ObservableProperty]
	private IEnumerable<ProductShort> _shorts = [];


	[RelayCommand]
	private async Task SearchAsync()
	{
		try
		{
			var shorts = await productService.SearchAsync(SearchTerm);
			Shorts = shorts;
		}
		catch (HttpServiceException)
		{
		}
	}

	[RelayCommand]
	private async Task ReturnAsync(ProductShort item)
	{
		if (!string.IsNullOrWhiteSpace(ReturnId))
		{
			var queries = new ShellNavigationQueryParameters()
			{
				{ ReturnId, item },
			};
			await NavigationService.BackAsync(queries);
		}
		else
		{
			await NavigationService.BackAsync();
		}
	}


	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.TryGetValue(QueryReturnId, out var returnId))
		{
			var casted = (string)returnId;
			ReturnId = casted;
		}

		if (query.TryGetValue(QuerySearchTerm, out var search))
		{
			var casted = (string)search;
			SearchTerm = casted;
			SearchCommand.Execute(null);
		}
	}
}
