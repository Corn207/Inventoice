using Android.Content.Res;

namespace MAUIApp;
public partial class App : Application
{
	public App(AppShell appShell)
	{
		Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (h, v) =>
		{
			h.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
		});

		InitializeComponent();

		UserAppTheme = AppTheme.Light;
		MainPage = appShell;
	}
}
