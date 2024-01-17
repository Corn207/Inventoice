using CommunityToolkit.Mvvm.ComponentModel;

namespace MAUIApp.Models.Clients;

public partial class Details : ObservableObject
{
	[ObservableProperty]
	private string _name = string.Empty;

	[ObservableProperty]
	private string _phonenumber = string.Empty;

	[ObservableProperty]
	private string? _email;

	[ObservableProperty]
	private string? _address;

	[ObservableProperty]
	private string? _description;

	public string? Id { get; init; }
	public DateTime? DateCreated { get; init; }
}
