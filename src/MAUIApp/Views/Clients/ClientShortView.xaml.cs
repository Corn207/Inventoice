namespace MAUIApp.Views.Clients;

public partial class ClientShortView : ContentView
{
	public ClientShortView()
	{
		InitializeComponent();
	}


	public static readonly BindableProperty NameProperty =
		BindableProperty.Create(nameof(Name), typeof(string), typeof(ClientShortView), null, BindingMode.OneTime);
	public string? Name
	{
		get => (string?)GetValue(NameProperty);
		set => SetValue(NameProperty, value);
	}

	public static readonly BindableProperty PhonenumberProperty =
		BindableProperty.Create(nameof(Phonenumber), typeof(string), typeof(ClientShortView), null, BindingMode.OneTime);
	public string? Phonenumber
	{
		get => (string?)GetValue(PhonenumberProperty);
		set => SetValue(PhonenumberProperty, value);
	}
}
