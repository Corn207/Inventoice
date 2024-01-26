using Domain.DTOs.ExportReports;

namespace MAUIApp.Models.ExportReports;
public record GroupShort(DateOnly Date, IReadOnlyList<ExportReportShort> Shorts);
