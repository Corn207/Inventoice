namespace Domain.DTOs.ExportReports;
public readonly record struct ExportReportCreate(
	string AuthorUserId,
	ExportReportCreateProductItem[] ProductItems);

public readonly record struct ExportReportCreateProductItem(
	string Id,
	uint Quantity);
