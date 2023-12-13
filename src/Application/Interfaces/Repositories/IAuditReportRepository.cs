﻿using Application.Interfaces.Repositories.Bases;
using Domain.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IAuditReportRepository : ISoftDeletableRepository<AuditReport>
{
	Task<List<AuditReport>> SearchAsync(
		string nameOrBarcode,
		Pagination pagination,
		TimeRange timeRange,
		OrderBy orderBy);
}
