namespace MAUIApp.Views;

public partial class TotalItemBar : ContentView
{
	public TotalItemBar()
	{
		InitializeComponent();
	}

	public static readonly BindableProperty TotalSearchProperty =
		BindableProperty.Create(nameof(TotalSearch), typeof(uint?), typeof(TotalItemBar), null);
	public uint? TotalSearch
	{
		get => (uint?)GetValue(TotalSearchProperty);
		set => SetValue(TotalSearchProperty, value);
	}

	public static readonly BindableProperty TotalAllProperty =
		BindableProperty.Create(nameof(TotalAll), typeof(uint?), typeof(TotalItemBar), null);
	public uint? TotalAll
	{
		get => (uint?)GetValue(TotalAllProperty);
		set => SetValue(TotalAllProperty, value);
	}

	public static readonly BindableProperty UnitTitleProperty =
		BindableProperty.Create(nameof(UnitTitle), typeof(string), typeof(TotalItemBar), null);
	public string? UnitTitle
	{
		get => (string?)GetValue(UnitTitleProperty);
		set => SetValue(UnitTitleProperty, value);
	}
}
