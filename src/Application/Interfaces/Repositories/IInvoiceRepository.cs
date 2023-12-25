using Application.Interfaces.Repositories.Bases;
using Domain.DTOs;
using Domain.DTOs.Invoices;
using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IInvoiceRepository : ISoftDeletableRepository<Invoice>
{
	Task<List<Invoice>> SearchAsync(
		string productNameOrBarcode,
		string clientNameOrPhonenumber,
		string authorName,
		InvoiceStatus status,
		TimeRange timeRange,
		OrderBy orderBy,
		Pagination pagination);
	Task<uint> CountAsync(
		string productNameOrBarcode,
		string clientNameOrPhonenumber,
		string authorName,
		InvoiceStatus status,
		TimeRange timeRange);
}
