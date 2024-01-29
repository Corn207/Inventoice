using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Identity.Domain.Entity;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;

namespace MAUIApp.ViewModels.Users;

public partial class EditAdminViewModel(
	IIdentityService identityService) : ObservableObject, IQueryAttributable
{
	public const string RouteName = "UserEditAdmin";
	public const string QueryId = "id";
	public const string QueryRole = "role";

	private string? _id;
	private Role Role
	{
		get
		{
			Role role = 0;
			if (IsAdmin)
			{
				role |= Role.Admin;
			}
			if (IsManager)
			{
				role |= Role.Manager;
			}
			if (IsEmployee)
			{
				role |= Role.Employee;
			}
			return role;
		}
	}

	[ObservableProperty]
	private bool _isAdmin = false;

	[ObservableProperty]
	private bool _isManager = false;

	[ObservableProperty]
	private bool _isEmployee = false;


	[RelayCommand]
	private async Task SaveAsync()
	{
		if (_id is null)
		{
			return;
		}
		if (!(IsAdmin || IsManager || IsEmployee))
		{
			await NavigationService.DisplayAlertAsync("Lỗi thao tác", "Phải chọn ít nhất một quyền hạn.", "Ok");
		}

		try
		{
			await identityService.UpdateRoleAsync(_id, Role);
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
		if (query.TryGetValue(QueryId, out var id))
		{
			var casted = (string)id;
			_id = casted;
		}

		if (query.TryGetValue(QueryRole, out var role))
		{
			var casted = (Role)role;
			IsAdmin = casted.HasFlag(Role.Admin);
			IsManager = casted.HasFlag(Role.Manager);
			IsEmployee = casted.HasFlag(Role.Employee);
		}
	}
}
