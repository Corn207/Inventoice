namespace Domain.DTOs.ExportReports;
public readonly record struct ExportReportCreate(
	ExportReportCreateProductItem[] ProductItems);

public readonly record struct ExportReportCreateProductItem(
	string Id,
	uint Quantity);
