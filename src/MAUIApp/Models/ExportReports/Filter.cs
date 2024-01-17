using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs;

namespace MAUIApp.Models.ExportReports;
public partial class Filter : ObservableObject
{
	[ObservableProperty]
	private string? _productNameOrBarcode;

	[ObservableProperty]
	private string? _authorName;

	[ObservableProperty]
	private DateTime? _dateStart;

	[ObservableProperty]
	private DateTime? _dateEnd;

	[ObservableProperty]
	private OrderBy _orderBy = OrderBy.Descending;
}
