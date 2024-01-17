using CommunityToolkit.Mvvm.ComponentModel;
using Domain.DTOs;
using Domain.DTOs.Invoices;
using MAUIApp.Models.Interfaces;

namespace MAUIApp.Models.Invoices;
public partial class Filter : ObservableObject, IProductNameBarcode
{
	[ObservableProperty]
	private string? _productNameOrBarcode;

	[ObservableProperty]
	private string? _clientNameOrPhonenumber;

	[ObservableProperty]
	private string? _authorName;

	[ObservableProperty]
	private InvoiceStatus _status = InvoiceStatus.All;

	[ObservableProperty]
	private DateTime? _dateStart;

	[ObservableProperty]
	private DateTime? _dateEnd;

	[ObservableProperty]
	private OrderBy _orderBy = OrderBy.Descending;
}
