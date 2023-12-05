using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Parameters;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;
public class ProductRepository : Repository<Product>, IProductRepository
{
	public ProductRepository(Database database) : base(database)
	{
	}

	public async Task<List<Product>> SearchAsync(
		string nameOrBarcode,
		PaginationParameters pagination,
		ProductOrderByParameter orderBy,
		bool isDescending)
	{
		var query = Database.Collection<Product>().AsQueryable();

		if (!string.IsNullOrWhiteSpace(nameOrBarcode))
		{
			query = query.Where(p => p.Barcode.Contains(nameOrBarcode) || p.Name.Contains(nameOrBarcode));
		}

		Expression<Func<Product, object>> expression = orderBy switch
		{
			ProductOrderByParameter.DateCreated => x => x.DateCreated,
			_ => x => x.Name,
		};

		if (isDescending)
		{
			query = query.OrderByDescending(expression);
		}
		else
		{
			query = query.OrderBy(expression);
		}

		var entities = await query
			.Skip((pagination.PageNumber - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return entities;
	}
}
