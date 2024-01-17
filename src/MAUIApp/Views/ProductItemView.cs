
namespace MAUIApp.Views;

public partial class ProductItemView : ContentView
{
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

	public static readonly BindableProperty InStockProperty =
		BindableProperty.Create(nameof(InStock), typeof(long?), typeof(ProductItemView), null);
	public long? InStock
	{
		get => (long?)GetValue(InStockProperty);
		set => SetValue(InStockProperty, value);
	}

	public static readonly BindableProperty PriceProperty =
		BindableProperty.Create(nameof(Price), typeof(long?), typeof(ProductItemView), null, propertyChanged: NotifyTotalPriceChanged);
	public long? Price
	{
		get => (long?)GetValue(PriceProperty);
		set => SetValue(PriceProperty, value);
	}

	public static readonly BindableProperty QuantityProperty =
		BindableProperty.Create(nameof(Quantity), typeof(long?), typeof(ProductItemView), null, propertyChanged: NotifyTotalPriceChanged);
	public long? Quantity
	{
		get => (long?)GetValue(QuantityProperty);
		set => SetValue(QuantityProperty, value);
	}

	public long? TotalPrice => (Price is null || Quantity is null) ? null : Price * Quantity;


	private static void NotifyTotalPriceChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is ProductItemView productItemView)
		{
			productItemView.OnPropertyChanged(nameof(TotalPrice));
		}
	}
}
