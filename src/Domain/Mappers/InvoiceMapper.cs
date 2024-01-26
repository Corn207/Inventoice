using Domain.DTOs.Invoices;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Domain.Mappers;
[Mapper]
public static partial class InvoiceMapper
{
	public static InvoiceShort ToShort(Invoice source)
	{
		if (source.ProductItems.Count == 0)
		{
			throw new InvalidOperationException("Invoice has no product items.");
		}

		var status = InvoiceStatus.Pending;
		if (source.DateCancelled.HasValue)
		{
			status = InvoiceStatus.Cancelled;
		}
		else if (source.DatePaid.HasValue)
		{
			status = InvoiceStatus.Paid;
		}

		var target = new InvoiceShort()
		{
			Id = source.Id ?? throw new NullReferenceException("Invoice's Id is null."),
			Status = status,
			ClientName = source.Client?.Name,
			DateCreated = source.DateCreated,
			GrandTotal = source.GrandTotal,
			TotalProduct = Convert.ToUInt32(source.ProductItems.Count),
			FirstProductName = source.ProductItems[0].Name,
			FirstProductQuantity = source.ProductItems[0].Quantity
		};

		return target;
	}
}
