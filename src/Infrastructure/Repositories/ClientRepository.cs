using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Repositories;
public class ClientRepository(Database database)
	: Repository<Client>(database), IClientRepository
{
	public async Task<List<Client>> SearchAsync(
		string nameOrPhonenumber,
		OrderBy orderBy,
		Pagination pagination)
	{
		var query = Database.Collection<Client>();
		var pipelineBuilder = new PipelineBuilder<Client>()
			.MatchOr(
				(nameof(Client.Name), nameOrPhonenumber),
				(nameof(Client.Phonenumber), nameOrPhonenumber))
			.Sort(nameof(Client.Name), orderBy)
			.Paging(pagination);
		var pipeline = pipelineBuilder.Build();
		var result = await query.Aggregate(pipeline).ToListAsync();

		return result;
	}

	public async Task<uint> CountAsync(
		string nameOrPhonenumber)
	{
		var query = Database.Collection<Client>();
		var pipelineBuilder = new PipelineBuilder<Client>()
			.MatchOr(
				(nameof(Client.Name), nameOrPhonenumber),
				(nameof(Client.Phonenumber), nameOrPhonenumber));
		return await pipelineBuilder.BuildAndCount(query);
	}
}
