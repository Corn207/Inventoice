using Domain.DTOs.Invoices;
using MAUIApp.Models.Products;

namespace MAUIApp.Mappers;

public static class InvoiceMapper
{
	public static InvoiceCreate ToCreate(
		string authorUserId,
		string? clientId,
		uint grandTotal,
		bool isPaid,
		IEnumerable<ProductItem> source)
	{
		var items = source.Select(x => new InvoiceCreateProductItem()
		{
			Id = x.Id,
			Quantity = x.Quantity,
		});
		var target = new InvoiceCreate()
		{
			AuthorUserId = authorUserId,
			ClientId = clientId,
			GrandTotal = grandTotal,
			IsPaid = isPaid,
			ProductItems = items.ToArray()
		};

		return target;
	}
}
