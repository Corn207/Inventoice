using Domain.DTOs;
using Domain.DTOs.AuditReports;
using Domain.DTOs.ImportReports;
using Domain.Entities;
using Domain.Mappers;
using MAUIApp.Services.Interfaces;

namespace MAUIApp.Services.Mocks;

public sealed class ImportReportService : IImportReportService
{
	private readonly List<ImportReport> _entities =
	[
		new ImportReport()
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
				new ImportReportProductItem()
				{
					Id = "Avxm1",
					Name = "Thuốc trị",
					Barcode = "02957293735313",
					Price = 16000,
					Quantity = 12,
				},
				new ImportReportProductItem()
				{
					Id = "39dnb",
					Name = "Dược phẩm",
					Barcode = "5432687452164",
					Price = 254000,
					Quantity = 4,
				},
			]
		},
		new ImportReport()
		{
			Id = "6532a",
			DateCreated = DateTime.Now - TimeSpan.FromDays(3),
			DateCancelled = DateTime.Now - TimeSpan.FromDays(1),
			Author = new UserInfo()
			{
				Id = "1nva",
				Name = "Nguyễn Văn A",
			},
			ProductItems =
			[
				new ImportReportProductItem()
				{
					Id = "Avxm1",
					Name = "Thuốc trị",
					Barcode = "02957293735313",
					Price = 16000,
					Quantity = 12,
				},
			]
		},
		new ImportReport()
		{
			Id = "13265",
			DateCreated = DateTime.Now - TimeSpan.FromDays(1),
			Author = new UserInfo()
			{
				Id = "1nva",
				Name = "Nguyễn Văn A",
			},
			ProductItems =
			[
				new ImportReportProductItem()
				{
					Id = "Avxm1",
					Name = "Thuốc trị",
					Barcode = "02957293735313",
					Price = 12000,
					Quantity = 54,
				},
				new ImportReportProductItem()
				{
					Id = "39dnb",
					Name = "Dược phẩm",
					Barcode = "5432687452164",
					Price = 78500,
					Quantity = 8,
				},
			]
		},
	];

	public Task<ImportReportShort[]> SearchAsync(
		string? productSearch = null,
		OrderBy orderBy = OrderBy.Descending,
		DateTime? dateStart = null,
		DateTime? dateEnd = null,
		ushort pageNumber = 1,
		ushort pageSize = 15)
	{
		var results = _entities.Select(ImportReportMapper.ToShort).ToArray();

		return Task.FromResult(results);
	}

	public Task<ImportReport> GetAsync(string id)
	{
		var result = _entities.First(x => x.Id == id);

		return Task.FromResult(result);
	}

	public Task CreateAsync(ImportReportCreate data)
	{
		return Task.CompletedTask;
	}

	public Task DeleteAsync(string id)
	{
		return Task.CompletedTask;
	}

	public Task CancelAsync(string id)
	{
		return Task.CompletedTask;
	}
}
