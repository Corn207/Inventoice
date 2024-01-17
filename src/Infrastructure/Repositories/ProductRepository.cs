using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;

namespace Infrastructure.Repositories;
public class ProductRepository(Database database)
	: Repository<Product>(database), IProductRepository
{
	public async Task<List<Product>> SearchAsync(
		string? nameOrBarcode,
		OrderBy orderBy,
		Pagination pagination)
	{
		var filters = new List<FilterDefinition<Product>>();
		filters.AddTextSearchCaseInsentitive(
			(x => x.Name, nameOrBarcode),
			(x => x.Barcode, nameOrBarcode));

		var result = await Database.Collection<Product>()
			.And(filters)
			.Sort(x => x.Name, orderBy)
			.Paginate(pagination)
			.ToListAsync();

		return result;
	}
}
