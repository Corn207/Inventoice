namespace Domain.DTOs.ExportReports;
public record struct ExportReportShort
{
	public required string Id { get; set; }
	public required string AuthorName { get; set; }
	public ushort TotalProduct { get; set; }
	public DateTime DateCreated { get; set; }
}
