using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Domain.Entities;
using Identity.Domain.DTOs;
using Identity.Domain.Entity;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;
using MAUIApp.ViewModels.Messenger;

namespace MAUIApp.ViewModels.Users;

public partial class DetailsAdminViewModel(
	IUserService userService,
	IIdentityService identityService) : ObservableObject, IQueryAttributable
{
	public const string RouteName = "UserDetailsAdmin";
	public const string QueryId = "id";
	public const string QueryRefresh = "refresh";

	private string? _id;

	[ObservableProperty]
	private User? _user;

	[ObservableProperty]
	private IdentityDetails? _identityDetails;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(Roles))]
	[NotifyCanExecuteChangedFor(nameof(ToEditAdminPageCommand))]
	[NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
	private string? _username;

	[ObservableProperty]
	private Role[] _roles = [];

	[ObservableProperty]
	private bool _isRefreshing = false;

	public bool CanExecuteResetPassword => User is not null;
	public bool CanExecuteToEditAdminPage => User is not null;
	public bool CanExecuteDelete => User is not null;


	[RelayCommand]
	private async Task LoadDataAsync()
	{
		if (_id is not null)
		{
			try
			{
				User = await userService.GetAsync(_id);
				IdentityDetails = await identityService.GetAsync(_id);
				Username = IdentityDetails.Value.Username;
				Roles = Enum.GetValues<Role>().Where(role => IdentityDetails.Value.Roles.HasFlag(role)).ToArray();
			}
			catch (HttpServiceException)
			{
				await NavigationService.BackAsync();
				return;
			}
		}

		IsRefreshing = false;
	}

	[RelayCommand(CanExecute = nameof(CanExecuteResetPassword))]
	private async Task ResetPasswordAsync()
	{
		if (_id is not null)
		{
			try
			{
				var newPassword = await identityService.ResetPasswordAsync(_id);
				await NavigationService.DisplayAlertAsync("Thành công", $"Mật khẩu mới là: {newPassword}", "OK");
			}
			catch (HttpServiceException)
			{
				return;
			}
		}
	}

	[RelayCommand(CanExecute = nameof(CanExecuteToEditAdminPage))]
	private async Task ToEditAdminPageAsync()
	{
		var role = Roles.Aggregate((Role)0, (current, next) => current | next);
		var queries = new ShellNavigationQueryParameters()
		{
			{ EditAdminViewModel.QueryId, _id! },
			{ EditAdminViewModel.QueryRole, role }
		};
		await NavigationService.ToAsync(EditAdminViewModel.RouteName, queries);
	}

	[RelayCommand(CanExecute = nameof(CanExecuteDelete))]
	private async Task DeleteAsync()
	{
		try
		{
			await identityService.DeleteAsync(_id!);
		}
		catch (HttpServiceException)
		{
			return;
		}

		WeakReferenceMessenger.Default.Send(new UserListRefreshMessage());
		await NavigationService.BackAsync();
	}


	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		var isRequestingRefresh = false;

		if (query.TryGetValue(QueryId, out var id))
		{
			var casted = (string)id;
			_id = casted;
			isRequestingRefresh = true;
		}

		if (query.ContainsKey(QueryRefresh))
		{
			isRequestingRefresh = true;
		}

		IsRefreshing = isRequestingRefresh;
	}
}
