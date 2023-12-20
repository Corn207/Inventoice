using Domain.DTOs.AuditReports;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Domain.Mappers;
[Mapper]
public static partial class AuditReportMapper
{
	public static AuditReportShort ToShort(AuditReport source)
	{
		var target = new AuditReportShort()
		{
			Id = source.Id ?? throw new NullReferenceException("Id is null."),
			Author = UserMapper.ToShort(source.Author),
			TotalProduct = Convert.ToUInt32(source.ProductItems.Count),
			DateCreated = source.DateCreated,
		};

		return target;
	}
}
