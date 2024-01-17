using Domain.DTOs.Interfaces;

namespace Domain.DTOs.Invoices;
public readonly record struct InvoiceShort(
	string Id,
	InvoiceStatus Status,
	string? ClientName,
	DateTime DateCreated,
	uint GrandTotal,
	uint TotalProduct,
	string FirstProductName,
	uint FirstProductQuantity) : IDto;
