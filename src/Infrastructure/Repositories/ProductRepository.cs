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
		string nameOrBarcode,
		OrderBy orderBy,
		Pagination pagination)
	{
		var query = Database.Collection<Product>();
		var pipelineBuilder = new PipelineBuilder<Product>()
			.MatchOr(
				(nameof(Product.Name), nameOrBarcode),
				(nameof(Product.Barcode), nameOrBarcode))
			.Sort(nameof(Product.Name), orderBy)
			.Paging(pagination);
		var pipeline = pipelineBuilder.Build();
		var result = await query.Aggregate(pipeline).ToListAsync();

		return result;
	}

	public async Task<uint> CountAsync(
		string nameOrBarcode)
	{
		var query = Database.Collection<Product>();
		var pipelineBuilder = new PipelineBuilder<Product>()
			.MatchOr(
				(nameof(Product.Name), nameOrBarcode),
				(nameof(Product.Barcode), nameOrBarcode));
		return await pipelineBuilder.BuildAndCount(query);
	}
}
