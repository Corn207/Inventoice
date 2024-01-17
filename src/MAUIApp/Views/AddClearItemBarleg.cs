using System.Windows.Input;

namespace MAUIApp.Views;

public class AddClearItemBar : ContentView
{
	public static readonly BindableProperty AddCommandProperty =
		BindableProperty.Create(nameof(AddCommand), typeof(ICommand), typeof(AddClearItemBar), default(ICommand));
	public ICommand AddCommand
	{
		get => (ICommand)GetValue(AddCommandProperty);
		set => SetValue(AddCommandProperty, value);
	}

	public static readonly BindableProperty AddCommandParameterProperty =
		BindableProperty.Create(nameof(AddCommandParameter), typeof(object), typeof(AddClearItemBar), null);
	public object? AddCommandParameter
	{
		get { return GetValue(AddCommandParameterProperty); }
		set { SetValue(AddCommandParameterProperty, value); }
	}

	public static readonly BindableProperty ClearCommandProperty =
		BindableProperty.Create(nameof(ClearCommand), typeof(ICommand), typeof(AddClearItemBar), default(ICommand));
	public ICommand ClearCommand
	{
		get => (ICommand)GetValue(ClearCommandProperty);
		set => SetValue(ClearCommandProperty, value);
	}

	public static readonly BindableProperty ClearCommandParameterProperty =
		BindableProperty.Create(nameof(ClearCommandParameter), typeof(object), typeof(AddClearItemBar), null);
	public object? ClearCommandParameter
	{
		get { return GetValue(ClearCommandParameterProperty); }
		set { SetValue(ClearCommandParameterProperty, value); }
	}
}
