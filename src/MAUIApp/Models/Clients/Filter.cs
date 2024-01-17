using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs;

namespace MAUIApp.Models.Clients;
public partial class Filter : ObservableObject
{
	[ObservableProperty]
	private string? _nameOrPhonenumber;

	[ObservableProperty]
	private OrderBy _orderBy = OrderBy.Ascending;
}
