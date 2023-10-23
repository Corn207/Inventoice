using Core.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Repositories;
public class ProductRepository
{
	public enum OrderedBy
	{
		Name, DateCreated
	}

	private readonly Database _database;

	public ProductRepository(Database database)
	{
		_database = database;
	}

	public async Task<List<Product>> SearchAsync(
		PaginationParameters pagination,
		string? nameOrBarcode = null,
		OrderedBy ordered = OrderedBy.Name,
		bool isDescending = false)
	{
		var query = _database.Products.AsQueryable();

		if (!string.IsNullOrWhiteSpace(nameOrBarcode))
		{
			query = query.Where(p => p.Barcode.Contains(nameOrBarcode) || p.Name.Contains(nameOrBarcode));
		}

		System.Linq.Expressions.Expression<Func<Product, object>> exp = ordered switch
		{
			OrderedBy.DateCreated => x => x.DateCreated,
			_ => x => x.Name,
		};

		if (isDescending)
		{
			query = query.OrderByDescending(exp);
		}
		else
		{
			query = query.OrderBy(exp);
		}

		var products = await query
			.Skip((pagination.PageNumber - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return products;
	}

	public async Task<Product?> GetAsync(string id)
	{
		var product = await _database.Products
			.Find(x => x.Id == id)
			.FirstOrDefaultAsync();

		return product;
	}

	public async Task<string> CreateAsync(Product product)
	{
		await _database.Products.InsertOneAsync(product);
		if (string.IsNullOrWhiteSpace(product.Id))
		{
			throw new NullReferenceException("Id is empty");
		}
		return product.Id;
	}

	public async Task<bool> ReplaceAsync(string id, Product product)
	{
		var result = await _database.Products.ReplaceOneAsync(x => x.Id == id, product);
		return result.ModifiedCount > 0;
	}

	public async Task<bool> DeleteAsync(string id)
	{
		var result = await _database.Products.DeleteOneAsync(x => x.Id == id);
		return result.DeletedCount > 0;
	}
}
