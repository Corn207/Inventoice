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

public partial class CreateUpdateViewModel(IClientService clientService) : ObservableObject, IQueryAttributable
{
	public const string RouteName = "ClientCreateUpdate";
	public const string QueryModel = "model";

	[ObservableProperty]
	private Details _model = new();


	[RelayCommand]
	private async Task SaveAsync()
	{
		var create = ClientMapper.ToCreateUpdate(Model);
		try
		{
			if (string.IsNullOrWhiteSpace(Model.Id))
			{
				await clientService.CreateAsync(create);
			}
			else
			{
				await clientService.UpdateAsync(Model.Id, create);
			}
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
		if (query.TryGetValue(QueryModel, out var model))
		{
			var casted = (Details)model;
			Model = casted;
		}
	}
}
