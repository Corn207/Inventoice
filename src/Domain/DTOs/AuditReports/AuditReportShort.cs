namespace Domain.DTOs.AuditReports;
public record struct AuditReportShort
{
	public required string Id { get; set; }
	public required string AuthorName { get; set; }
	public ushort TotalProduct { get; set; }
	public DateTime DateCreated { get; set; }
}
