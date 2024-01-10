using Domain.DTOs;
using Domain.DTOs.Invoices;
using Domain.Entities;

namespace MAUIApp.Services.HttpServices.Interfaces;

public interface IInvoiceService : IService<Invoice>, ICancellableService
{
    Task<InvoiceShort[]> SearchAsync(
        string? productNameOrBarcode = null,
        string? clientNameOrPhonenumber = null,
        string? authorName = null,
        InvoiceStatus status = InvoiceStatus.All,
        DateTime? dateStart = null,
        DateTime? dateEnd = null,
        OrderBy orderBy = OrderBy.Descending,
        ushort pageNumber = 1,
        ushort pageSize = 15,
		CancellationToken cancellationToken = default);
    Task<uint> CountAsync(
        string? productNameOrBarcode = null,
        string? clientNameOrPhonenumber = null,
        string? authorName = null,
        InvoiceStatus status = InvoiceStatus.All,
        DateTime? dateStart = null,
        DateTime? dateEnd = null,
		CancellationToken cancellationToken = default);
    Task CreateAsync(
        InvoiceCreate body,
		CancellationToken cancellationToken = default);
    Task PayAsync(
        string id,
		CancellationToken cancellationToken = default);
}
