namespace Domain.DTOs.AuditReports;
public readonly record struct AuditReportCreate(
	string AuthorUserId,
	AuditReportCreateProductItem[] ProductItems);

public readonly record struct AuditReportCreateProductItem(
	string Id,
	uint Quantity);
