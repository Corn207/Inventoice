namespace Domain.DTOs.ImportReports;
public readonly record struct ImportReportCreate(
	string AuthorUserId,
	ImportReportCreateProductItem[] ProductItems);

public readonly record struct ImportReportCreateProductItem(
	string Id,
	uint Price,
	uint Quantity);
