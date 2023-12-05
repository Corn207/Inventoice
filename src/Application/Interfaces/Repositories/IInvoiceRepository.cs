using Application.Interfaces.Repositories.Bases;
using Domain.DTOs.Invoices;
using Domain.Entities;
using Domain.Parameters;

namespace Application.Interfaces.Repositories;
public interface IInvoiceRepository : ISoftDeletableRepository<Invoice>
{
	Task<List<InvoiceShort>> SearchAsync(
		InvoiceFilterParameters filter,
		PaginationParameters pagination,
		TimeRangeParameters timeRange,
		bool isDescending);
}
