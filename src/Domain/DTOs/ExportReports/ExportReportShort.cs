using Domain.DTOs.Users;

namespace Domain.DTOs.ExportReports;
public readonly record struct ExportReportShort(
	string Id,
	UserShort Author,
	uint TotalProduct,
	DateTime DateCreated);
