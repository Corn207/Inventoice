using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Repositories;
public class ProductRepository(Database database)
	: Repository<Product>(database), IProductRepository
{
	private IMongoQueryable<Product> GetSearchQuery(
		string nameOrBarcode,
		OrderBy orderBy)
	{
		var query = Database.Collection<Product>().AsQueryable();

		if (!string.IsNullOrWhiteSpace(nameOrBarcode))
		{
			query = query.Where(p =>
			p.Barcode.Contains(nameOrBarcode, StringComparison.InvariantCultureIgnoreCase) ||
			p.Name.Contains(nameOrBarcode, StringComparison.InvariantCultureIgnoreCase));
		}

		if (orderBy == OrderBy.Ascending)
		{
			query = query.OrderBy(x => x.DateCreated);
		}
		else
		{
			query = query.OrderByDescending(x => x.DateCreated);
		}

		return query;
	}

	public async Task<List<Product>> SearchAsync(
		string nameOrBarcode,
		OrderBy orderBy,
		Pagination pagination)
	{
		var query = GetSearchQuery(nameOrBarcode, orderBy);

		var result = await query
			.Skip((pagination.PageNumber - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return result;
	}

	public async Task<uint> CountAsync(
		string nameOrBarcode,
		OrderBy orderBy)
	{
		var query = GetSearchQuery(nameOrBarcode, orderBy);

		var result = await query.CountAsync();

		return Convert.ToUInt32(result);
	}
}
