namespace MAUIApp.Views;

public partial class ValueField : ContentView
{
	public ValueField()
	{
		InitializeComponent();
	}

	public static readonly BindableProperty TitleProperty =
		BindableProperty.Create(nameof(Title), typeof(string), typeof(ValueField), string.Empty);
	public string Title
	{
		get => (string)GetValue(TitleProperty);
		set => SetValue(TitleProperty, value);
	}

	public static readonly BindableProperty ValueTextColorProperty =
		BindableProperty.Create(nameof(ValueTextColor), typeof(Color), typeof(ValueField), null);
	public Color? ValueTextColor
	{
		get => (Color?)GetValue(ValueTextColorProperty);
		set => SetValue(ValueTextColorProperty, value);
	}

	public static readonly BindableProperty ValueProperty =
		BindableProperty.Create(nameof(Value), typeof(string), typeof(ValueField), null);
	public string? Value
	{
		get => (string?)GetValue(ValueProperty);
		set => SetValue(ValueProperty, value);
	}

	public static readonly BindableProperty FormattedTextProperty =
		BindableProperty.Create(nameof(FormattedText), typeof(FormattedString), typeof(ValueField), default(FormattedString));
	public FormattedString FormattedText
	{
		get { return (FormattedString)GetValue(FormattedTextProperty); }
		set { SetValue(FormattedTextProperty, value); }
	}
}
