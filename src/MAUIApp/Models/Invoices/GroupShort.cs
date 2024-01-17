using Domain.DTOs.Invoices;

namespace MAUIApp.Models.Invoices;
public record GroupShort(DateOnly Date, IReadOnlyList<InvoiceShort> Shorts);
