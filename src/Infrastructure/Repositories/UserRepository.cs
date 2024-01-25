using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Repositories;
public class UserRepository(Database database)
	: Repository<User>(database), IUserRepository
{
	public async Task<PartialEnumerable<User>> SearchAsync(
		string? name,
		OrderBy orderBy,
		Pagination pagination)
	{
		var query = Database.Collection<User>();
		PipelineDefinition<User, User> pipeline = new EmptyPipelineDefinition<User>();

		if (!string.IsNullOrWhiteSpace(name))
		{
			var filter = Builders<User>.Filter.Eq(x => x.Name, name);
			pipeline = pipeline.Match(filter);
		}

		var sortStage = Utility.BuildStageSort<User>(x => x.Name, orderBy);
		pipeline = pipeline.AppendStage(sortStage);

		var groupStage = Utility.BuildStageGroupAndPage<User>(pagination);
		var finalPipeline = pipeline.AppendStage(groupStage);

		var result = await query.Aggregate(finalPipeline).FirstOrDefaultAsync();
		result ??= new PartialEnumerable<User>([], 0);

		return result;
	}
}
