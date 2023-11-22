using Domain.DTOs.ImportReports;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Domain.Mappers;
[Mapper]
public static partial class ImportReportMapper
{
	public static ImportReportShort ToShortForm(ImportReport source)
	{
		var target = new ImportReportShort()
		{
			Id = source.Id!,
			AuthorName = source.Author.Name,
			TotalProduct = (ushort)source.ProductItems.Count,
			DateCreated = source.DateCreated
		};

		return target;
	}
}
