namespace MAUIApp.Views;

public partial class FilterTime : Card
{
	public FilterTime()
	{
		InitializeComponent();
	}

	public static readonly BindableProperty FromProperty =
		BindableProperty.Create(nameof(From), typeof(DateTime?), typeof(FilterTime), null, BindingMode.TwoWay, propertyChanged: FromToPropertyChanged);
	public DateTime? From
	{
		get => (DateTime?)GetValue(FromProperty);
		set => SetValue(FromProperty, value);
	}

	public static readonly BindableProperty ToProperty =
		BindableProperty.Create(nameof(To), typeof(DateTime?), typeof(FilterTime), null, BindingMode.TwoWay, propertyChanged: FromToPropertyChanged);
	public DateTime? To
	{
		get => (DateTime?)GetValue(ToProperty);
		set => SetValue(ToProperty, value);
	}
	private static void FromToPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var obj = (FilterTime)bindable;
		if (obj.SelectedOptionIndex != -1)
			obj.SelectedOptionIndex = -1;
	}


	private int _selectedOptionIndex = -1;
	public int SelectedOptionIndex
	{
		get => _selectedOptionIndex;
		set
		{
			_selectedOptionIndex = value;
			OnPropertyChanged();
		}
	}

	private void OptionSelectedIndexChanged(object sender, EventArgs e)
	{
		switch (SelectedOptionIndex)
		{
			case 0:
				SetValue(FromProperty, null);
				SetValue(ToProperty, null);
				break;
			case 1:
				SetValue(FromProperty, DateTime.UtcNow.Date);
				SetValue(ToProperty, null);
				break;
			case 2:
				SetValue(FromProperty, DateTime.UtcNow.AddDays(-1).Date);
				SetValue(ToProperty, null);
				break;
			case 3:
				SetValue(FromProperty, DateTime.UtcNow.AddDays(1 - (int)DateTime.UtcNow.DayOfWeek).Date);
				SetValue(ToProperty, null);
				break;
			case 4:
				SetValue(FromProperty, DateTime.UtcNow.AddDays(1 - (int)DateTime.UtcNow.DayOfWeek - 7).Date);
				SetValue(ToProperty, null);
				break;
			case 5:
				SetValue(FromProperty, new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1));
				SetValue(ToProperty, null);
				break;
			case 6:
				SetValue(FromProperty, new DateTime(DateTime.UtcNow.Year, 1, 1));
				SetValue(ToProperty, null);
				break;
			default:
				break;
		}
	}
}
