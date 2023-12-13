using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.DTOs.Products;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;
public class ProductRepository(Database database) : Repository<Product>(database), IProductRepository
{
	public async Task<List<Product>> SearchAsync(
		string nameOrBarcode,
		Pagination pagination,
		ProductSortBy sortBy,
		OrderBy orderBy)
	{
		var query = Database.Collection<Product>().AsQueryable();

		if (!string.IsNullOrWhiteSpace(nameOrBarcode))
		{
			query = query.Where(p => p.Barcode.Contains(nameOrBarcode) || p.Name.Contains(nameOrBarcode));
		}

		Expression<Func<Product, object>> expression = sortBy switch
		{
			ProductSortBy.DateCreated => x => x.DateCreated,
			_ => x => x.Name,
		};

		if (orderBy == OrderBy.Ascending)
		{
			query = query.OrderBy(expression);
		}
		else
		{
			query = query.OrderByDescending(expression);
		}

		var entities = await query
			.Skip((pagination.PageNumber - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return entities;
	}
}
