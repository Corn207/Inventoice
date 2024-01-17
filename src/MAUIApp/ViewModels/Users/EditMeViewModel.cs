using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.DTOs.Users;
using MAUIApp.Models.Users;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;

namespace MAUIApp.ViewModels.Users;

public partial class EditMeViewModel(
	IUserService userService) : ObservableObject, IQueryAttributable
{
	public const string RouteName = "UserEditMe";
	public const string QueryModel = "model";

	[ObservableProperty]
	private UpdateMe _model = new();


	[RelayCommand]
	private async Task SaveAsync()
	{
		if (string.IsNullOrWhiteSpace(Model.Name) || string.IsNullOrWhiteSpace(Model.Phonenumber))
		{
			return;
		}

		var body = new UserCreateUpdate()
		{
			Name = Model.Name,
			Phonenumber = Model.Phonenumber,
		};
		try
		{
			await userService.EditMeAsync(body);
		}
		catch (HttpServiceException)
		{
			return;
		}

		var queries = new Dictionary<string, string>()
		{
			{ DetailsAdminViewModel.QueryRefresh, string.Empty }
		};
		await NavigationService.BackAsync(queries);
	}


	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.TryGetValue(QueryModel, out var model))
		{
			var casted = (UpdateMe)model;
			Model = casted;
		}
	}
}
