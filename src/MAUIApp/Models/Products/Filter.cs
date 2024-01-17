using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs;

namespace MAUIApp.Models.Products;
public partial class Filter : ObservableObject
{
	[ObservableProperty]
	private string? _productNameOrBarcode;

	[ObservableProperty]
	private OrderBy _orderBy = OrderBy.Ascending;
}
