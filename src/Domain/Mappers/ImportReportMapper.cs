using Domain.DTOs.ImportReports;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Domain.Mappers;
[Mapper]
public static partial class ImportReportMapper
{
	public static ImportReportShort ToShort(ImportReport source)
	{
		var target = new ImportReportShort()
		{
			Id = source.Id ?? throw new NullReferenceException("ImportReport's ID is null."),
			Author = UserMapper.ToShort(source.Author),
			TotalProduct = Convert.ToUInt32(source.ProductItems.Count),
			TotalPrice = Convert.ToUInt32(source.ProductItems.Sum(x => x.Quantity * x.Price)),
			DateCreated = source.DateCreated
		};

		return target;
	}
}
