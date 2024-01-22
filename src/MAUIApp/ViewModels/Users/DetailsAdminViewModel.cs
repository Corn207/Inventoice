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

	private string? _id;

	[ObservableProperty]
	private User? _user;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(Roles))]
	[NotifyCanExecuteChangedFor(nameof(ToEditAdminPageCommand))]
	[NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
	private IdentityDetails? _identity;

	[ObservableProperty]
	private bool _isRefreshing = false;

	public IEnumerable<Role> Roles =>
		Identity.HasValue ?
			Enum.GetValues<Role>().Where(role => Identity.Value.Roles.HasFlag(role))
			: Enumerable.Empty<Role>();
	public bool CanExecuteToEditAdminPage => User is not null && Identity is not null;
	public bool CanExecuteDelete => User is not null && Identity is not null;


	[RelayCommand]
	private async Task LoadDataAsync()
	{
		if (_id is not null)
		{
			try
			{
				User = await userService.GetAsync(_id);
				Identity = await identityService.GetAsync(_id);
			}
			catch (HttpServiceException)
			{
				await NavigationService.BackAsync();
				return;
			}
		}

		IsRefreshing = false;
	}

	[RelayCommand(CanExecute = nameof(CanExecuteToEditAdminPage))]
	private async Task ToEditAdminPageAsync()
	{
		//var queries = new ShellNavigationQueryParameters()
		//{
		//	{ CreateUpdateViewModel.QueryModel, Model! }
		//};
		//await NavigationService.ToAsync(CreateUpdateViewModel.RouteName, queries);
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
		if (query.TryGetValue(QueryId, out var id))
		{
			var casted = (string)id;
			_id = casted;
			IsRefreshing = true;
		}
	}
}
