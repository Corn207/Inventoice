namespace MAUIApp.Views;

public partial class EditProductItemHeaderView : ContentView
{
	public EditProductItemHeaderView()
	{
		InitializeComponent();
	}

	public static readonly BindableProperty NameProperty =
		BindableProperty.Create(nameof(Name), typeof(string), typeof(EditProductItemHeaderView), null);
	public string? Name
	{
		get => (string?)GetValue(NameProperty);
		set => SetValue(NameProperty, value);
	}

	public static readonly BindableProperty BarcodeProperty =
		BindableProperty.Create(nameof(Barcode), typeof(string), typeof(EditProductItemHeaderView), null);
	public string? Barcode
	{
		get => (string?)GetValue(BarcodeProperty);
		set => SetValue(BarcodeProperty, value);
	}
}
