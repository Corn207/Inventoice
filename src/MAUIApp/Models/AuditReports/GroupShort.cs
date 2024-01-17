using Domain.DTOs.AuditReports;

namespace MAUIApp.Models.AuditReports;
public record GroupShort(DateOnly Date, IReadOnlyList<AuditReportShort> Shorts);
