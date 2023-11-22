namespace Domain.DTOs.ImportReports;
public record struct ImportReportCreate
{
	public required string AuthorUserId { get; set; }
	public ImportReportProductItemCreate[] ProductItems { get; set; }
}

public record struct ImportReportProductItemCreate
{
	public required string ProductId { get; set; }
	public uint UnitPrice { get; set; }
	public uint Quantity { get; set; }
}
