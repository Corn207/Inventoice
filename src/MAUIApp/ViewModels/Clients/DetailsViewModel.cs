using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MAUIApp.Mappers;
using MAUIApp.Models.Clients;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;
using MAUIApp.ViewModels.Messenger;

namespace MAUIApp.ViewModels.Clients;

public partial class DetailsViewModel(IClientService clientService) : ObservableObject, IQueryAttributable
{
	public const string RouteName = "ClientDetails";
	public const string QueryId = "id";

	[ObservableProperty]
	private string? _id;

	[ObservableProperty]
	private Details? _model;

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
				var entity = await clientService.GetAsync(Id);
				Model = ClientMapper.ToDetails(entity);
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
			await clientService.DeleteAsync(Id!);
		}
		catch (HttpServiceException)
		{
			return;
		}

		WeakReferenceMessenger.Default.Send(new ClientListRefreshMessage());
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
