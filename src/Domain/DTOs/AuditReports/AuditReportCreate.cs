namespace Domain.DTOs.AuditReports;
public record struct AuditReportCreate
{
	public required string AuthorUserId { get; set; }
	public AuditReportProductItemCreate[] ProductItems { get; set; }
}

public record struct AuditReportProductItemCreate
{
	public required string ProductId { get; set; }
	public uint AdjustedQuantity { get; set; }
}
