using Domain.DTOs.ExportReports;
using MAUIApp.Models.Products;

namespace MAUIApp.Mappers;

public static class ExportReportMapper
{
	public static ExportReportCreate ToCreate(string authorUserId, IEnumerable<ProductItem> source)
	{
		var items = source.Select(x => new ExportReportCreateProductItem(x.Id, x.Quantity));
		var target = new ExportReportCreate(authorUserId, items.ToArray());

		return target;
	}
}
