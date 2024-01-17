using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using MAUIApp.Models.Users;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;

namespace MAUIApp.ViewModels.Users;

public partial class DetailsUserViewModel(
	NavigationService navigationService,
	IUserService userService) : ObservableObject, IQueryAttributable
{
	public const string RouteName = "UserDetailsUser";
	public const string QueryRefresh = "refresh";

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(ChangePasswordCommand))]
	private User? _user;

	[ObservableProperty]
	private bool _isRefreshing = true;

	public bool CanExecuteChangePassword => User is not null;
	public bool CanExecuteToEditMePage => User is not null;


	[RelayCommand]
	private async Task LoadDataAsync()
	{
		try
		{
			User = await userService.GetMeAsync();

		}
		catch (HttpServiceException)
		{
			await NavigationService.BackAsync();
			return;
		}

		IsRefreshing = false;
	}

	[RelayCommand(CanExecute = nameof(CanExecuteChangePassword))]
	private async Task ChangePasswordAsync()
	{
		await navigationService.ShowPopup<ChangePasswordViewModel>();
	}

	[RelayCommand(CanExecute = nameof(CanExecuteToEditMePage))]
	private async Task ToEditMePageAsync()
	{
		var model = new UpdateMe()
		{
			Name = User!.Name,
			Phonenumber = User!.Phonenumber,
		};
		var queries = new ShellNavigationQueryParameters()
		{
			{ EditMeViewModel.QueryModel, model }
		};
		await NavigationService.ToAsync(EditMeViewModel.RouteName, queries);
	}


	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.ContainsKey(QueryRefresh))
		{
			IsRefreshing = true;
		}
	}
}
