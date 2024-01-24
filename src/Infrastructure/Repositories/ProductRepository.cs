using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;

namespace Infrastructure.Repositories;
public class ProductRepository(Database database)
	: Repository<Product>(database), IProductRepository
{
	public async Task<PartialEnumerable<Product>> SearchAsync(
		string? nameOrBarcode,
		OrderBy orderBy,
		Pagination pagination)
	{
		var query = Database.Collection<Product>();
		PipelineDefinition<Product, Product> pipeline = new EmptyPipelineDefinition<Product>();

		if (!string.IsNullOrWhiteSpace(nameOrBarcode))
		{
			var filter = Builders<Product>.Filter.Or(
				Utility.TextSearchCaseInsentitive<Product>(x => x.Name, nameOrBarcode),
				Utility.TextSearchCaseInsentitive<Product>(x => x.Barcode, nameOrBarcode));
			pipeline = pipeline.Match(filter);
		}

		var sortStage = Utility.BuildStageSort<Product>(x => x.Name, orderBy);
		pipeline = pipeline.AppendStage(sortStage);

		var groupStage = Utility.BuildStageGroupAndPage<Product>(pagination);
		var finalPipeline = pipeline.AppendStage(groupStage);

		var result = await query.Aggregate(finalPipeline).FirstAsync();

		return result;
	}
}
