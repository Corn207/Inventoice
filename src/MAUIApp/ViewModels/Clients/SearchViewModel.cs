using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.DTOs.Clients;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;

namespace MAUIApp.ViewModels.Clients;

public partial class SearchViewModel(IClientService clientService) : ObservableObject, IQueryAttributable
{
	public const string RouteName = "ClientSearch";
	public const string QueryReturnId = "returnId";
	public const string QuerySearchTerm = "search";

	public string? ReturnId { get; set; }

	[ObservableProperty]
	private string _searchTerm = string.Empty;

	[ObservableProperty]
	private ClientShort[] _shorts = [];


	[RelayCommand]
	private async Task SearchAsync()
	{
		try
		{
			Shorts = await clientService.SearchAsync(SearchTerm);
		}
		catch (HttpServiceException)
		{
		}
	}

	[RelayCommand]
	private async Task ReturnAsync(ClientShort item)
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
