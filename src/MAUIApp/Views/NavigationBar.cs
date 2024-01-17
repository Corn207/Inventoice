using System.Windows.Input;

namespace MAUIApp.Views;

public class NavigationBar : ContentView
{
	#region Back and Title
	public static readonly BindableProperty BackCommandProperty =
		BindableProperty.Create(nameof(BackCommand), typeof(ICommand), typeof(NavigationBar));
	public ICommand BackCommand
	{
		get => (ICommand)GetValue(BackCommandProperty);
		set => SetValue(BackCommandProperty, value);
	}
	
	public static readonly BindableProperty BackSourceProperty =
		BindableProperty.Create(nameof(BackSource), typeof(ImageSource), typeof(NavigationBar), ImageSource.FromFile("arrow_back_ios_new.png"));
	public ImageSource BackSource
	{
		get => (ImageSource)GetValue(BackSourceProperty);
		set => SetValue(BackSourceProperty, value);
	}
	
	public static readonly BindableProperty BackHeightRequestProperty =
		BindableProperty.Create(nameof(BackHeightRequest), typeof(double), typeof(NavigationBar), (double)16);
	public double BackHeightRequest
	{
		get => (double)GetValue(BackHeightRequestProperty);
		set => SetValue(BackHeightRequestProperty, value);
	}

	public static readonly BindableProperty BackMarginProperty =
		BindableProperty.Create(nameof(BackMargin), typeof(Thickness), typeof(NavigationBar), new Thickness(0, 0, 16, 0));
	public Thickness BackMargin
	{
		get => (Thickness)GetValue(BackMarginProperty);
		set => SetValue(BackMarginProperty, value);
	}

	public static readonly BindableProperty IsBackButtonVisibleProperty =
		BindableProperty.Create(nameof(IsBackButtonVisible), typeof(bool), typeof(NavigationBar), true);
	public bool IsBackButtonVisible
	{
		get => (bool)GetValue(IsBackButtonVisibleProperty);
		set => SetValue(IsBackButtonVisibleProperty, value);
	}

	public static readonly BindableProperty TitleProperty =
		BindableProperty.Create(nameof(Title), typeof(string), typeof(NavigationBar), string.Empty); 
	public string Title
	{
		get => (string)GetValue(TitleProperty);
		set => SetValue(TitleProperty, value);
	}
	#endregion


	#region Content
	public static readonly BindableProperty BodyPaddingProperty =
		BindableProperty.Create(nameof(BodyPadding), typeof(Thickness), typeof(NavigationBar), new Thickness(16, 16, 8, 16));
	public Thickness BodyPadding
	{
		get => (Thickness)GetValue(BodyPaddingProperty);
		set => SetValue(BodyPaddingProperty, value);
	}
	
	public static readonly BindableProperty ContentHorizontalOptionsProperty =
		BindableProperty.Create(nameof(ContentHorizontalOptions), typeof(LayoutOptions), typeof(NavigationBar), LayoutOptions.End);
	public LayoutOptions ContentHorizontalOptions
	{
		get => (LayoutOptions)GetValue(ContentHorizontalOptionsProperty);
		set => SetValue(ContentHorizontalOptionsProperty, value);
	}
	#endregion


	#region Footer
	public static readonly BindableProperty FooterProperty =
	BindableProperty.Create(nameof(Footer), typeof(IView), typeof(NavigationBar), default(IView));
	public IView Footer
	{
		get => (IView)GetValue(FooterProperty);
		set => SetValue(FooterProperty, value);
	}

	public static readonly BindableProperty FooterPaddingProperty =
		BindableProperty.Create(nameof(FooterPadding), typeof(Thickness), typeof(NavigationBar), new Thickness(16, 8));
	public Thickness FooterPadding
	{
		get => (Thickness)GetValue(FooterPaddingProperty);
		set => SetValue(FooterPaddingProperty, value);
	}
	#endregion
}
