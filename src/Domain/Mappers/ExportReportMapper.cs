using Domain.DTOs.ExportReports;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Domain.Mappers;
[Mapper]
public static partial class ExportReportMapper
{
	public static ExportReportShort ToShort(ExportReport source)
	{
		var target = new ExportReportShort()
		{
			Id = source.Id ?? throw new NullReferenceException("ExportReport's Id is null."),
			Author = UserMapper.ToShort(source.Author),
			TotalProduct = Convert.ToUInt32(source.ProductItems.Count),
			DateCreated = source.DateCreated
		};

		return target;
	}
}
