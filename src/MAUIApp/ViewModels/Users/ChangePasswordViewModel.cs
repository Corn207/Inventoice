using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Identity.Domain.DTOs;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;

namespace MAUIApp.ViewModels.Users;

public partial class ChangePasswordViewModel(
	IIdentityService identityService) : ObservableObject
{
	public event EventHandler? OnChangePasswordSuccess;

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(SaveCommand))]
	private string? _currentPassword;

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(SaveCommand))]
	private string? _newPassword;

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(SaveCommand))]
	private string? _confirmPassword;

	public bool CanExecuteSave =>
		!string.IsNullOrWhiteSpace(CurrentPassword) &&
		!string.IsNullOrWhiteSpace(NewPassword) &&
		!string.IsNullOrWhiteSpace(ConfirmPassword);


	[RelayCommand(CanExecute = nameof(CanExecuteSave))]
	private async Task SaveAsync()
	{
		if (NewPassword is null || ConfirmPassword is null || CurrentPassword is null)
		{
			await NavigationService.DisplayAlertAsync("Lỗi thao tác", "Nhập thiếu trường dữ liệu", "OK");
			return;
		}
		if (NewPassword != ConfirmPassword)
		{
			await NavigationService.DisplayAlertAsync("Lỗi thao tác", "Mật khẩu mới không khớp", "OK");
			return;
		}

		var body = new ChangePassword(CurrentPassword, NewPassword);

		try
		{
			await identityService.ChangePasswordAsync(body);
		}
		catch (HttpServiceException)
		{
			return;
		}

		await NavigationService.DisplayAlertAsync("Thành công", "Đổi mật khẩu thành công", "OK");
		OnChangePasswordSuccess?.Invoke(this, EventArgs.Empty);
	}
}
