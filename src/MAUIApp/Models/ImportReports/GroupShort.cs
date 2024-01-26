using Domain.DTOs.ImportReports;

namespace MAUIApp.Models.ImportReports;
public record GroupShort(DateOnly Date, IReadOnlyList<ImportReportShort> Shorts);
