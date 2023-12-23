using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Repositories;
public class ExportReportRepository(Database database)
	: SoftDeletableRepository<ExportReport>(database), IExportReportRepository
{
	private IMongoQueryable<ExportReport> GetSearchQuery(
		string productNameOrBarcode,
		string authorName,
		TimeRange timeRange,
		OrderBy orderBy)
	{
		var query = Database.Collection<ExportReport>().AsQueryable()
			.Where(x => x.DateDeleted == null);

		if (!string.IsNullOrWhiteSpace(productNameOrBarcode))
		{
			query = query.Where(x => x.ProductItems.Any(i =>
				i.Name.Contains(productNameOrBarcode, StringComparison.InvariantCultureIgnoreCase) ||
				i.Barcode.Contains(productNameOrBarcode, StringComparison.InvariantCultureIgnoreCase)));
		}

		if (!string.IsNullOrWhiteSpace(authorName))
		{
			query = query.Where(x => x.Author.Name.Contains(authorName, StringComparison.InvariantCultureIgnoreCase));
		}

		if (timeRange.From != DateTime.MinValue)
		{
			query = query.Where(p => p.DateCreated >= timeRange.From);
		}

		if (timeRange.To != DateTime.MaxValue)
		{
			query = query.Where(p => p.DateCreated <= timeRange.To);
		}

		if (orderBy == OrderBy.Ascending)
		{
			query = query.OrderBy(p => p.DateCreated);
		}
		else
		{
			query = query.OrderByDescending(p => p.DateCreated);
		}

		return query;
	}

	public async Task<List<ExportReport>> SearchAsync(
		string productNameOrBarcode,
		string authorName,
		TimeRange timeRange,
		OrderBy orderBy,
		Pagination pagination)
	{
		var query = GetSearchQuery(productNameOrBarcode, authorName, timeRange, orderBy);

		var result = await query
			.Skip((pagination.PageNumber - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return result;
	}

	public async Task<uint> CountAsync(
		string productNameOrBarcode,
		string authorName,
		TimeRange timeRange,
		OrderBy orderBy)
	{
		var query = GetSearchQuery(productNameOrBarcode, authorName, timeRange, orderBy);

		var result = await query.CountAsync();

		return Convert.ToUInt32(result);
	}
}
