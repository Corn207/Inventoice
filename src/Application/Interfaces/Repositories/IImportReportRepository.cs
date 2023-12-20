﻿using Application.Interfaces.Repositories.Bases;
using Domain.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IImportReportRepository : ISoftDeletableRepository<ImportReport>
{
	Task<List<ImportReport>> SearchAsync(
		string productNameOrBarcode,
		TimeRange timeRange,
		OrderBy orderBy,
		Pagination pagination);
}
