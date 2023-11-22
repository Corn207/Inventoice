using Application.Interfaces.Repositories.Bases;
using Application.Interfaces.Repositories.Parameters;
using Domain.DTOs.Invoices;
using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IInvoiceRepository : ISoftDeletableRepository<Invoice>
{
	Task<List<InvoiceShort>> SearchAsync(
		InvoiceFilterParameters filter,
		PaginationParameters pagination,
		TimeRangeParameters timeRange,
		bool isDescending);
}
