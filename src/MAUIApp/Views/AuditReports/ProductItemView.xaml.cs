namespace MAUIApp.Views.AuditReports;

public partial class ProductItemView : ContentView
{
	public ProductItemView()
	{
		InitializeComponent();
	}


	public static readonly BindableProperty NameProperty =
		BindableProperty.Create(nameof(Name), typeof(string), typeof(ProductItemView), null);
	public string? Name
	{
		get => (string?)GetValue(NameProperty);
		set => SetValue(NameProperty, value);
	}

	public static readonly BindableProperty BarcodeProperty =
		BindableProperty.Create(nameof(Barcode), typeof(string), typeof(ProductItemView), null);
	public string? Barcode
	{
		get => (string?)GetValue(BarcodeProperty);
		set => SetValue(BarcodeProperty, value);
	}

	public static readonly BindableProperty OriginalQuantityProperty =
		BindableProperty.Create(nameof(OriginalQuantity), typeof(uint?), typeof(ProductItemView), null, propertyChanged: QuantityChanged);
	public uint? OriginalQuantity
	{
		get => (uint?)GetValue(OriginalQuantityProperty);
		set => SetValue(OriginalQuantityProperty, value);
	}

	public static readonly BindableProperty AdjustedQuantityProperty =
		BindableProperty.Create(nameof(AdjustedQuantity), typeof(uint?), typeof(ProductItemView), null, propertyChanged: QuantityChanged);
	public uint? AdjustedQuantity
	{
		get => (uint?)GetValue(AdjustedQuantityProperty);
		set => SetValue(AdjustedQuantityProperty, value);
	}

	private static void QuantityChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is ProductItemView view)
		{
			view.OnPropertyChanged(nameof(Difference));
		}
	}

	public long? Difference => (AdjustedQuantity is null || OriginalQuantity is null) ? null : (long)AdjustedQuantity - OriginalQuantity;
}
