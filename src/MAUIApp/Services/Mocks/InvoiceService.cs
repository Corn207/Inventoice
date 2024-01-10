using Domain.DTOs;
using Domain.DTOs.Invoices;
using Domain.Entities;
using MAUIApp.Services.Interfaces;

namespace MAUIApp.Services.Mocks;

public class InvoiceService : IInvoiceService
{
	private readonly Invoice[] _entities =
	[
		new()
		{
			Id = "12sv",
			Client = new()
			{
				Id = "1",
				Name = "Quang Đại",
				Phonenumber = "1234567890",
			},
			DateCreated = DateTime.Parse("08/18/2018 07:22:16"),
			GrandTotal = 2750000,
			Author = new()
			{
				Id = "1",
				Name = "Test"
			},
			ProductItems =
			[
				new()
				{
					Id = "1",
					Barcode = "8651245732563",
					Name = "Sữa rửa mặt",
					Price = 132000,
					Quantity = 20
				},
				new()
				{
					Id = "2",
					Barcode = "8453215796532",
					Name = "Thuốc ho nong đờm",
					Price = 52000,
					Quantity = 10
				},
				new()
				{
					Id = "3",
					Barcode = "8453215796532",
					Name = "Thuốc",
					Price = 10000,
					Quantity = 1
				},
				new()
				{
					Id = "3",
					Barcode = "8453215796532",
					Name = "Thuốc",
					Price = 10000,
					Quantity = 1
				},
				new()
				{
					Id = "3",
					Barcode = "8453215796532",
					Name = "Thuốc",
					Price = 10000,
					Quantity = 1
				},
				new()
				{
					Id = "3",
					Barcode = "8453215796532",
					Name = "Thuốc",
					Price = 10000,
					Quantity = 1
				},
				new()
				{
					Id = "3",
					Barcode = "8453215796532",
					Name = "Thuốc",
					Price = 10000,
					Quantity = 1
				},
				new()
				{
					Id = "3",
					Barcode = "8453215796532",
					Name = "Thuốc",
					Price = 10000,
					Quantity = 1
				},
				new()
				{
					Id = "3",
					Barcode = "8453215796532",
					Name = "Thuốc",
					Price = 10000,
					Quantity = 1
				},
				new()
				{
					Id = "3",
					Barcode = "8453215796532",
					Name = "Thuốc",
					Price = 10000,
					Quantity = 1
				},
				new()
				{
					Id = "3",
					Barcode = "8453215796532",
					Name = "Thuốc",
					Price = 10000,
					Quantity = 1
				}
			],
			ExportReportId = "1",
		}
	];

	private readonly InvoiceShort[] _shorts =
	[
		new("12sv", InvoiceStatus.Pending, "Nguyễn Văn A", DateTime.Parse("08/18/2018 07:22:16"), 126000, 3, "Sữa rửa mặt", 2),
		new("6jhj", InvoiceStatus.Paid, "Văn B", DateTime.Parse("04/21/2022 18:22:22"), 2630000, 30, "Trà thanh nhiệt", 30),
		new("290x", InvoiceStatus.Cancelled, null, DateTime.Parse("11/04/2023 00:00:00"), 10000, 1, "Thuốc", 1),
		new("63aa", InvoiceStatus.Cancelled, null, DateTime.Parse("11/04/2023 00:00:00"), 10090000, 10, "Thuốc ho", 100)
	];

	public Task<InvoiceShort[]> SearchAsync(
		string? productNameOrBarcode = null,
		string? clientNameOrPhonenumber = null,
		string? authorName = null,
		InvoiceStatus status = InvoiceStatus.All,
		OrderBy orderBy = OrderBy.Descending,
		DateTime? dateStart = null,
		DateTime? dateEnd = null,
		ushort pageNumber = 1,
		ushort pageSize = 15)
	{
		return Task.FromResult(_shorts);
	}

	public Task<Invoice> GetAsync(string id)
	{
		var result = _entities.First();

		return Task.FromResult(result);
	}

	public Task CancelAsync(string id)
	{
		return Task.CompletedTask;
	}

	public Task CreateAsync(InvoiceCreate data)
	{
		return Task.CompletedTask;
	}

	public Task DeleteAsync(string id)
	{
		return Task.CompletedTask;
	}

	public Task PayAsync(string id)
	{
		return Task.CompletedTask;
	}
}
