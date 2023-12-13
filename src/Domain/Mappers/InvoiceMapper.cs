using Domain.DTOs.Invoices;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Domain.Mappers;
[Mapper]
public static partial class InvoiceMapper
{
	public static InvoiceShort ToShortForm(Invoice source)
	{
		var status = InvoiceStatus.Pending;
		if (source.DateCancelled != null)
		{
			status = InvoiceStatus.Cancelled;
		}
		else if (source.DatePaid != null)
		{
			status = InvoiceStatus.Paid;
		}

		var target = new InvoiceShort()
		{
			Id = source.Id!,
			Status = status,
			ClientName = source.Client?.Name,
			DateCreated = source.DateCreated,
			GrandTotal = source.GrandTotal,
			TotalProduct = (ushort)source.ProductItems.Count,
			FirstProductName = source.ProductItems[0].Name,
			FirstProductQuantity = source.ProductItems[0].Quantity
		};

		return target;
	}
}
