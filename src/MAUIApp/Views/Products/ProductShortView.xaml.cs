namespace MAUIApp.Views.Products;

public partial class ProductShortView : ContentView
{
	public ProductShortView()
	{
		InitializeComponent();
	}


	public static readonly BindableProperty NameProperty =
		BindableProperty.Create(nameof(Name), typeof(string), typeof(ProductItemView), null, BindingMode.OneTime);
	public string? Name
	{
		get => (string?)GetValue(NameProperty);
		set => SetValue(NameProperty, value);
	}

	public static readonly BindableProperty BarcodeProperty =
		BindableProperty.Create(nameof(Barcode), typeof(string), typeof(ProductItemView), null, BindingMode.OneTime);
	public string? Barcode
	{
		get => (string?)GetValue(BarcodeProperty);
		set => SetValue(BarcodeProperty, value);
	}

	public static readonly BindableProperty InStockProperty =
		BindableProperty.Create(nameof(InStock), typeof(uint?), typeof(ProductItemView), null, BindingMode.OneTime);
	public uint? InStock
	{
		get => (uint?)GetValue(InStockProperty);
		set => SetValue(InStockProperty, value);
	}

	public static readonly BindableProperty PriceProperty =
		BindableProperty.Create(nameof(Price), typeof(uint?), typeof(ProductItemView), null, BindingMode.OneTime);
	public uint? Price
	{
		get => (uint?)GetValue(PriceProperty);
		set => SetValue(PriceProperty, value);
	}
}
