using Domain.DTOs.AuditReports;
using MAUIApp.Models.Products;

namespace MAUIApp.Mappers;

public static class AuditReportMapper
{
	public static AuditReportCreate ToCreate(string authorUserId, IEnumerable<ProductItem> source)
	{
		var items = source.Select(x => new AuditReportCreateProductItem(x.Id, x.Quantity));
		var target = new AuditReportCreate(authorUserId, items.ToArray());

		return target;
	}
}
