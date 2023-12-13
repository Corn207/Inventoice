namespace Domain.DTOs.Invoices;
public readonly record struct InvoiceShort(
	string Id,
	InvoiceStatus Status,
	string? ClientName,
	DateTime DateCreated,
	uint GrandTotal,
	ushort TotalProduct,
	string FirstProductName,
	uint FirstProductQuantity);
