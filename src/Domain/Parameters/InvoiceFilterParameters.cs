using Domain.DTOs.Invoices;

namespace Domain.Parameters;
public readonly struct InvoiceFilterParameters
{
	public InvoiceFilterParameters(
		string productNameOrBarcode,
		string clientNameOrPhonenumber,
		string authorName,
		InvoiceStatus? status)
	{
		ProductNameOrBarcode = productNameOrBarcode;
		ClientNameOrPhonenumber = clientNameOrPhonenumber;
		AuthorName = authorName;
		Status = status;
	}

	public readonly string ProductNameOrBarcode;
	public readonly string ClientNameOrPhonenumber;
	public readonly string AuthorName;
	public readonly InvoiceStatus? Status;
}
