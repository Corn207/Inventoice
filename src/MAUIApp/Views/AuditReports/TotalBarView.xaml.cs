namespace MAUIApp.Views.AuditReports;

public partial class TotalBarView : ContentView
{
	public TotalBarView()
	{
		InitializeComponent();
	}


	public static readonly BindableProperty TotalProductProperty =
		BindableProperty.Create(nameof(TotalProduct), typeof(uint?), typeof(TotalBarView), null);
	public uint? TotalProduct
	{
		get => (uint?)GetValue(TotalProductProperty);
		set => SetValue(TotalProductProperty, value);
	}

	public static readonly BindableProperty TotalOriginalProperty =
		BindableProperty.Create(nameof(TotalOriginal), typeof(uint?), typeof(TotalBarView), null, propertyChanged: TotalQuantityChanged);
	public uint? TotalOriginal
	{
		get => (uint?)GetValue(TotalOriginalProperty);
		set => SetValue(TotalOriginalProperty, value);
	}

	public static readonly BindableProperty TotalAdjustedProperty =
		BindableProperty.Create(nameof(TotalAdjusted), typeof(uint?), typeof(TotalBarView), null, propertyChanged: TotalQuantityChanged);
	public uint? TotalAdjusted
	{
		get => (uint?)GetValue(TotalAdjustedProperty);
		set => SetValue(TotalAdjustedProperty, value);
	}

	private static void TotalQuantityChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is TotalBarView totalBar)
		{
			totalBar.OnPropertyChanged(nameof(TotalDifference));
		}
	}

	public long? TotalDifference => (TotalAdjusted is null || TotalOriginal is null) ? null : (long)TotalAdjusted - TotalOriginal;
}
