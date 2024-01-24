using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;

namespace Infrastructure.Repositories;
public class ClientRepository(Database database)
	: Repository<Client>(database), IClientRepository
{
	public async Task<PartialEnumerable<Client>> SearchAsync(
		string? nameOrPhonenumber,
		OrderBy orderBy,
		Pagination pagination)
	{
		var query = Database.Collection<Client>();
		PipelineDefinition<Client, Client> pipeline = new EmptyPipelineDefinition<Client>();

		if (!string.IsNullOrWhiteSpace(nameOrPhonenumber))
		{
			var filter = Builders<Client>.Filter.Or(
				Utility.TextSearchCaseInsentitive<Client>(x => x.Name, nameOrPhonenumber),
				Utility.TextSearchCaseInsentitive<Client>(x => x.Phonenumber, nameOrPhonenumber));
			pipeline = pipeline.Match(filter);
		}

		var sortStage = Utility.BuildStageSort<Client>(x => x.Name, orderBy);
		pipeline = pipeline.AppendStage(sortStage);

		var groupStage = Utility.BuildStageGroupAndPage<Client>(pagination);
		var finalPipeline = pipeline.AppendStage(groupStage);

		var result = await query.Aggregate(finalPipeline).FirstAsync();

		return result;
	}
}
