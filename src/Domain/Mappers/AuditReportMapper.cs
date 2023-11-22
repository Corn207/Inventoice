using Domain.DTOs.AuditReports;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Domain.Mappers;
[Mapper]
public static partial class AuditReportMapper
{
	public static AuditReportShort ToShortForm(AuditReport source)
	{
		var target = new AuditReportShort()
		{
			Id = source.Id!,
			AuthorName = source.Author.Name,
			TotalProduct = (ushort)source.ProductItems.Count,
			DateCreated = source.DateCreated,
		};

		return target;
	}
}
