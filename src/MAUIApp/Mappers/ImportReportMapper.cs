using Domain.DTOs.ImportReports;
using MAUIApp.Models.Products;

namespace MAUIApp.Mappers;

public static class ImportReportMapper
{
	public static ImportReportCreate ToCreate(string authorUserId, IEnumerable<ProductItem> source)
	{
		var items = source
			.Select(x => new ImportReportCreateProductItem(x.Id, x.Price, x.Quantity))
			.ToArray();
		var target = new ImportReportCreate(authorUserId, items);

		return target;
	}
}
