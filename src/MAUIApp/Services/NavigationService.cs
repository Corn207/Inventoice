using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using MAUIApp.Views;
using System.ComponentModel;

namespace MAUIApp.Services;
public sealed class NavigationService(IPopupService popupService)
{
	#region To methods
	public static async Task ToAsync(string route)
	{
		await Shell.Current.GoToAsync(route);
	}

	public static async Task ToAsync(string route, string key, string value)
	{
		await Shell.Current.GoToAsync($"{route}?{key}={value}", true);
	}

	public static async Task ToAsync(string route, IDictionary<string, string> parameters)
	{
		var queryString = string.Join('&', parameters.Select(p => $"{p.Key}={p.Value}"));
		await Shell.Current.GoToAsync($"{route}?{queryString}", true);
	}

	public static async Task ToAsync(string route, IDictionary<string, object> parameters)
	{
		await Shell.Current.GoToAsync(route, true, parameters);
	}

	public static async Task ToAsync(string route, ShellNavigationQueryParameters parameters)
	{
		await Shell.Current.GoToAsync(route, true, parameters);
	}
	#endregion

	#region Back methods
	public static async Task BackAsync()
	{
		await ToAsync("..");
	}

	public static async Task BackAsync(IDictionary<string, string> parameters)
	{
		await ToAsync("..", parameters);
	}

	public static async Task BackAsync(IDictionary<string, object> parameters)
	{
		await ToAsync("..", parameters);
	}

	public static async Task BackAsync(ShellNavigationQueryParameters parameters)
	{
		await ToAsync("..", parameters);
	}

	public static async Task BackToRootAsync()
	{
		await Shell.Current.Navigation.PopToRootAsync();
	}
	#endregion


	public async Task ShowPopup<TViewModel>(Action<TViewModel>? action = null)
		where TViewModel : INotifyPropertyChanged
	{
		if (action is null)
		{
			await popupService.ShowPopupAsync<TViewModel>();
		}
		else
		{
			await popupService.ShowPopupAsync(action);
		}
	}

	#region Alerts
	public static async Task DisplayDuplicateProductAlert()
	{
		await Shell.Current.DisplayAlert("Trùng hàng", "Đã thêm sản phẩm này.", "OK");
	}

	public static async Task DisplayOutOfStockAlert()
	{
		await Shell.Current.DisplayAlert("Hết hàng", "Sản phẩm hết hàng trong kho.", "OK");
	}

	public static async Task DisplayAlertAsync(string title, string message, string cancel)
	{
		await Shell.Current.DisplayAlert(title, message, cancel);
	}
	#endregion

	#region Shell related
	public const string LoginStackRouteName = "loginStack";
	public const string MainStackRouteName = "mainStack";

	public static async Task ToLoginStackAsync()
	{
		await Shell.Current.GoToAsync($"//{LoginStackRouteName}");
	}

	public static async Task ToMainStackAsync()
	{
		await Shell.Current.GoToAsync($"//{MainStackRouteName}");
	}
	#endregion
}
