namespace Domain.DTOs.Invoices;

public readonly record struct InvoiceCreate(
	string AuthorUserId,
	string? ClientId,
	InvoiceCreateProductItem[] ProductItems,
	uint GrandTotal,
	bool IsPaid);

public readonly record struct InvoiceCreateProductItem(
	string Id,
	uint Quantity);
