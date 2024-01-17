using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUIApp.Services;

namespace MAUIApp.ViewModels.Products;

public partial class BarcodeScannerViewModel : ObservableObject, IQueryAttributable
{
	public const string RouteName = "BarcodeScanner";
	public const string QueryReturnId = "returnId";

	public string? ReturnId { get; set; }

	[ObservableProperty]
	private bool _pauseScanning = false;

	[ObservableProperty]
	private bool _cameraEnabled = true;

	[RelayCommand]
	private static async Task CloseAsync()
	{
		await NavigationService.BackAsync();
	}

	[RelayCommand]
	private static async Task LoadedAsync()
	{
		var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
		if (status != PermissionStatus.Granted)
		{
			status = await Permissions.RequestAsync<Permissions.Camera>();
			if (status != PermissionStatus.Granted)
			{
				await NavigationService.BackAsync();
				return;
			}
		}
	}

	[RelayCommand]
	private async Task DetectedAsync(BarcodeScanning.OnDetectionFinishedEventArg e)
	{
		var result = e.BarcodeResults.FirstOrDefault();
		if (result is null) return;

		PauseScanning = true;
		CameraEnabled = false;
		if (!string.IsNullOrWhiteSpace(ReturnId))
		{
			var query = new ShellNavigationQueryParameters()
			{
				{ ReturnId, result.RawValue },
			};
			await NavigationService.BackAsync(query);
		}
		else
		{
			await NavigationService.BackAsync();
		}
	}

	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.TryGetValue(QueryReturnId, out var returnId))
		{
			var casted = (string)returnId;
			ReturnId = casted;
		}
	}
}
