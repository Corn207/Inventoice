using Domain.DTOs.ExportReports;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Domain.Mappers;
[Mapper]
public static partial class ExportReportMapper
{
	public static ExportReportShort ToShortForm(ExportReport source)
	{
		var target = new ExportReportShort()
		{
			Id = source.Id!,
			AuthorName = source.Author.Name,
			TotalProduct = (ushort)source.ProductItems.Count,
			DateCreated = source.DateCreated
		};

		return target;
	}
}
