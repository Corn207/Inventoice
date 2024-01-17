using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs;

namespace MAUIApp.Models.Users;
public partial class Filter : ObservableObject
{
	[ObservableProperty]
	private string? _name;

	[ObservableProperty]
	private OrderBy _orderBy = OrderBy.Ascending;
}
