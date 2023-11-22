namespace Domain.DTOs.ExportReports;
public record struct ExportReportCreate
{
	public required string AuthorUserId { get; set; }
	public ExportReportProductItemCreate[] ProductItems { get; set; }
}

public record struct ExportReportProductItemCreate
{
	public required string ProductId { get; set; }
	public uint Quantity { get; set; }
}
