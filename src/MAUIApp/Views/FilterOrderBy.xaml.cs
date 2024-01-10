namespace MAUIApp.Views;

public partial class FilterOrderBy : Card
{
	public FilterOrderBy()
	{
		InitializeComponent();
	}


	public static readonly BindableProperty OrderByProperty =
		BindableProperty.Create(nameof(OrderBy), typeof(object), typeof(FilterOrderBy), Domain.DTOs.OrderBy.Ascending, BindingMode.TwoWay);
	public object OrderBy
	{
		get => (object)GetValue(OrderByProperty);
		set => SetValue(OrderByProperty, value);
	}

	public static readonly BindableProperty AscendingTitleProperty =
		BindableProperty.Create(nameof(AscendingTitle), typeof(string), typeof(FilterOrderBy), "Ascending");
	public string AscendingTitle
	{
		get => (string)GetValue(AscendingTitleProperty);
		set => SetValue(AscendingTitleProperty, value);
	}

	public static readonly BindableProperty DescendingTitleProperty =
		BindableProperty.Create(nameof(DescendingTitle), typeof(string), typeof(FilterOrderBy), "Descending");
	public string DescendingTitle
	{
		get => (string)GetValue(DescendingTitleProperty);
		set => SetValue(DescendingTitleProperty, value);
	}
}
