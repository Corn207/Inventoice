using Domain.DTOs.Users;

namespace Domain.DTOs.AuditReports;
public readonly record struct AuditReportShort(
	string Id,
	UserShort Author,
	uint TotalProduct,
	DateTime DateCreated);
