namespace MAUIApp.Views;

public partial class InventoryReportShortView : ContentView
{
	public InventoryReportShortView()
	{
		InitializeComponent();
	}


	public static readonly BindableProperty TotalProductProperty =
		BindableProperty.Create(nameof(TotalProduct), typeof(uint?), typeof(InventoryReportShortView), null);
	public uint? TotalProduct
	{
		get => (uint?)GetValue(TotalProductProperty);
		set => SetValue(TotalProductProperty, value);
	}

	public static readonly BindableProperty DateCreatedProperty =
		BindableProperty.Create(nameof(DateCreated), typeof(DateTime?), typeof(InventoryReportShortView), null);
	public DateTime? DateCreated
	{
		get => (DateTime?)GetValue(DateCreatedProperty);
		set => SetValue(DateCreatedProperty, value);
	}

	public static readonly BindableProperty AuthorNameProperty =
		BindableProperty.Create(nameof(AuthorName), typeof(string), typeof(InventoryReportShortView), null);
	public string? AuthorName
	{
		get => (string?)GetValue(AuthorNameProperty);
		set => SetValue(AuthorNameProperty, value);
	}

	public static readonly BindableProperty TotalPriceProperty =
		BindableProperty.Create(nameof(TotalPrice), typeof(uint?), typeof(InventoryReportShortView), null);
	public uint? TotalPrice
	{
		get => (uint?)GetValue(TotalPriceProperty);
		set => SetValue(TotalPriceProperty, value);
	}
}
