using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Identity.Domain.DTOs;
using Identity.Domain.Entity;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;
using MAUIApp.ViewModels.Messenger;

namespace MAUIApp.ViewModels.Users;

public partial class CreateViewModel(IIdentityService service) : ObservableObject
{
	public const string RouteName = "UserCreate";

	[ObservableProperty]
	private string _name = string.Empty;

	[ObservableProperty]
	private string _phonenumber = string.Empty;

	[ObservableProperty]
	private string _username = string.Empty;

	[ObservableProperty]
	private bool _hasAdminRole = false;

	[ObservableProperty]
	private bool _hasManagerRole = false;

	[ObservableProperty]
	private bool _hasEmployeeRole = true;


	[RelayCommand]
	private async Task SaveAsync()
	{
		if (string.IsNullOrWhiteSpace(Name))
		{
			await NavigationService.DisplayAlertAsync("Lỗi thao tác", "Tên không được để trống", "OK");
			return;
		}
		if (string.IsNullOrWhiteSpace(Phonenumber))
		{
			await NavigationService.DisplayAlertAsync("Lỗi thao tác", "Số điện thoại không được để trống", "OK");
			return;
		}
		if (string.IsNullOrWhiteSpace(Username))
		{
			await NavigationService.DisplayAlertAsync("Lỗi thao tác", "Tên đăng nhập không được để trống", "OK");
			return;
		}
		if (!(HasAdminRole || HasManagerRole || HasEmployeeRole))
		{
			await NavigationService.DisplayAlertAsync("Lỗi thao tác", "Phải chọn ít nhất một quyền hạn", "OK");
			return;
		}

		Role roles = 0;
		if (HasAdminRole)
		{
			roles |= Role.Admin;
		}
		if (HasManagerRole)
		{
			roles |= Role.Manager;
		}
		if (HasEmployeeRole)
		{
			roles |= Role.Employee;
		}

		var create = new CreateIdentity()
		{
			Name = Name,
			Phonenumber = Phonenumber,
			Username = Username,
			Roles = roles,
		};
		string password;
		try
		{
			password = await service.CreateAsync(create);
		}
		catch (HttpServiceException)
		{
			return;
		}

		await NavigationService.DisplayAlertAsync("Thành công", $"Tạo người dùng thành công, mật khẩu là {password}", "OK");

		WeakReferenceMessenger.Default.Send(new UserListRefreshMessage());
		await NavigationService.BackAsync();
	}
}
