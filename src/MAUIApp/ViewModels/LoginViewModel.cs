using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Identity.Domain.DTOs;
using MAUIApp.Pages;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;

namespace MAUIApp.ViewModels;
public partial class LoginViewModel(IIdentityService service) : ObservableObject
{
	public const string RouteName = "login";

	[ObservableProperty]
	private string _username = string.Empty;

	[ObservableProperty]
	private string _password = string.Empty;


	[RelayCommand]
	private async Task LoginAsync()
	{
		var body = new Login(Username, Password);
		try
		{
			await service.LoginAsync(body);
		}
		catch (InvalidLoginException)
		{
			await NavigationService.DisplayAlert("Đăng nhập không thành công", "Sai tên đăng nhập hoặc mật khẩu", "OK");
			return;
		}
		catch (ActionFailedException ex)
		{
			await NavigationService.DisplayAlert("Lỗi mạng", ex.Message, "OK");
			return;
		}

		await NavigationService.ToMainStackAsync();
	}

	[RelayCommand]
	private static async Task ToSettingsPageAsync()
	{
		await NavigationService.ToAsync(SettingsViewModel.RouteName);
	}
}
