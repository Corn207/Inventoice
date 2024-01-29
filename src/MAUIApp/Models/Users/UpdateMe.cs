using CommunityToolkit.Mvvm.ComponentModel;

namespace MAUIApp.Models.Users;
public partial class UpdateMe : ObservableObject
{
	[ObservableProperty]
	private string? _name;

	[ObservableProperty]
	private string? _phonenumber;
}
