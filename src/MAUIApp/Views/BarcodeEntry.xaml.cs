namespace MAUIApp.Views;

public partial class BarcodeEntry : ContentView
{
	public BarcodeEntry()
	{
		InitializeComponent();
	}


	public static readonly BindableProperty TextProperty =
		BindableProperty.Create(nameof(Text), typeof(string), typeof(BarcodeEntry), null, BindingMode.TwoWay);
	public string? Text
	{
		get => (string?)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public static readonly BindableProperty ReturnIdProperty =
		BindableProperty.Create(nameof(ReturnId), typeof(string), typeof(BarcodeEntry), null);
	public string? ReturnId
	{
		get => (string?)GetValue(ReturnIdProperty);
		set => SetValue(ReturnIdProperty, value);
	}
}
