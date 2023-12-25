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
	public async Task<List<User>> SearchAsync(
		string name,
		OrderBy orderBy,
		Pagination pagination)
	{
		var query = Database.Collection<User>();
		var pipelineBuilder = new PipelineBuilder<User>()
			.MatchOr((nameof(User.Name), name))
			.Sort(nameof(User.Name), orderBy)
			.Paging(pagination);
		var pipeline = pipelineBuilder.Build();
		var result = await query.Aggregate(pipeline).ToListAsync();

		return result;
	}

	public async Task<uint> CountAsync(
		string name)
	{
		var query = Database.Collection<User>();
		var pipelineBuilder = new PipelineBuilder<User>()
			.MatchOr((nameof(User.Name), name));
		var pipeline = pipelineBuilder.BuildCount();
		var result = await query.Aggregate(pipeline).FirstOrDefaultAsync();

		return Convert.ToUInt32(result.Count);
	}
}
