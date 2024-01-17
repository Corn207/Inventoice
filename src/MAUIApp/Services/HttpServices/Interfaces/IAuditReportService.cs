﻿using Domain.DTOs;
using Domain.DTOs.AuditReports;
using Domain.Entities;

namespace MAUIApp.Services.HttpServices.Interfaces;

public interface IAuditReportService : IService<AuditReport>
{
    Task<AuditReportShort[]> SearchAsync(
        string? productNameOrBarcode = null,
        string? authorName = null,
        DateTime? dateStart = null,
        DateTime? dateEnd = null,
        OrderBy orderBy = OrderBy.Descending,
        ushort pageNumber = 1,
        ushort pageSize = 15,
        CancellationToken cancellationToken = default);
    Task<uint> CountAsync(
        string? productNameOrBarcode = null,
        string? authorName = null,
        DateTime? dateStart = null,
        DateTime? dateEnd = null,
		CancellationToken cancellationToken = default);
    Task CreateAsync(
        AuditReportCreate body,
		CancellationToken cancellationToken = default);
}
