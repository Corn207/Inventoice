using Microsoft.Maui.Controls.Shapes;

namespace MAUIApp.Views;

public class Card : ContentView
{
	public static readonly BindableProperty HeaderTitleProperty =
		BindableProperty.Create(nameof(HeaderTitle), typeof(string), typeof(Card), null);
	public string? HeaderTitle
	{
		get => (string?)GetValue(HeaderTitleProperty);
		set => SetValue(HeaderTitleProperty, value);
	}

	public static readonly BindableProperty BodyPaddingProperty =
		BindableProperty.Create(nameof(BodyPadding), typeof(Thickness), typeof(Card), new Thickness(16, 8));
	public Thickness BodyPadding
	{
		get => (Thickness)GetValue(BodyPaddingProperty);
		set => SetValue(BodyPaddingProperty, value);
	}

	public static readonly BindableProperty StrokeShapeProperty =
		BindableProperty.Create(nameof(StrokeShape), typeof(IShape), typeof(Card), new RoundRectangle()
		{
			CornerRadius = new CornerRadius(16),
		});
	[System.ComponentModel.TypeConverter(typeof(StrokeShapeTypeConverter))]
	public IShape? StrokeShape
	{
		get => (IShape?)GetValue(StrokeShapeProperty);
		set => SetValue(StrokeShapeProperty, value);
	}
}
