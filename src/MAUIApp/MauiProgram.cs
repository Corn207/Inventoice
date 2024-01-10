using BarcodeScanning;
using CommunityToolkit.Maui;
using MAUIApp.Extensions;
using Microsoft.Extensions.Logging;
using The49.Maui.ContextMenu;

namespace MAUIApp;
public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.Services
			.AddServices()
			.AddViewsAndViewModels();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.UseBarcodeScanning()
			.UseContextMenu()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
#if DEBUG
		builder.Logging.AddDebug();
#endif
		return builder.Build();
	}
}
