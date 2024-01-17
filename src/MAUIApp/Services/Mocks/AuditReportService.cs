using Domain.DTOs;
using Domain.DTOs.AuditReports;
using Domain.Entities;
using Domain.Mappers;
using MAUIApp.Services.Interfaces;

namespace MAUIApp.Services.Mocks;

public sealed class AuditReportService : IAuditReportService
{
	private readonly List<AuditReport> _entities =
	[
		new AuditReport()
		{
			Id = "abm32",
			DateCreated = DateTime.Now - TimeSpan.FromDays(12),
			Author = new UserInfo()
			{
				Id = "1nva",
				Name = "Nguyễn Văn A",
			},
			ProductItems =
			[
				new AuditReportProductItem()
				{
					Id = "Avxm1",
					Name = "Thuốc trị",
					Barcode = "02957293735313",
					OriginalQuantity = 1,
					AdjustedQuantity = 12,
				},
				new AuditReportProductItem()
				{
					Id = "39dnb",
					Name = "Dược phẩm",
					Barcode = "5432687452164",
					OriginalQuantity = 3,
					AdjustedQuantity = 0,
				},
			]
		},
		new AuditReport()
		{
			Id = "20mnd",
			DateCreated = DateTime.Now - TimeSpan.FromDays(2),
			Author = new UserInfo()
			{
				Id = "1nva",
				Name = "Nguyễn Văn A",
			},
			ProductItems =
			[
				new AuditReportProductItem()
				{
					Id = "Avxm1",
					Name = "Thuốc xổ",
					Barcode = "02957293735313",
					OriginalQuantity = 6,
					AdjustedQuantity = 7,
				},
				new AuditReportProductItem()
				{
					Id = "39dnb",
					Name = "Dược phẩm",
					Barcode = "5432687452164",
					OriginalQuantity = 20,
					AdjustedQuantity = 15,
				},
			]
		},
		new AuditReport()
		{
			Id = "abm32",
			DateCreated = DateTime.Now - TimeSpan.FromDays(12),
			Author = new UserInfo()
			{
				Id = "1nva",
				Name = "Nguyễn Văn A",
			},
			ProductItems =
			[
				new AuditReportProductItem()
				{
					Id = "Avxm1",
					Name = "Thuốc xổ",
					Barcode = "02957293735313",
					OriginalQuantity = 3,
					AdjustedQuantity = 4,
				},
				new AuditReportProductItem()
				{
					Id = "39dnb",
					Name = "Dược phẩm",
					Barcode = "5432687452164",
					OriginalQuantity = 2,
					AdjustedQuantity = 1,
				},
			]
		},
	];

	public Task<AuditReportShort[]> SearchAsync(
		string? productSearch = null,
		OrderBy orderBy = OrderBy.Descending,
		DateTime? dateStart = null,
		DateTime? dateEnd = null,
		ushort pageNumber = 1,
		ushort pageSize = 15)
	{
		var results = _entities.Select(AuditReportMapper.ToShort).ToArray();

		return Task.FromResult(results);
	}

	public Task<AuditReport> GetAsync(string id)
	{
		var result = _entities.First(x => x.Id == id);

		return Task.FromResult(result);
	}

	public Task CreateAsync(AuditReportCreate data)
	{
		return Task.CompletedTask;
	}

	public Task DeleteAsync(string id)
	{
		return Task.CompletedTask;
	}
}
