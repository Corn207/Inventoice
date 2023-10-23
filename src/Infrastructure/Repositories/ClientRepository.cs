using Core.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories;
public class ClientRepository
{
	private readonly Database _database;

	public ClientRepository(Database database)
	{
		_database = database;
	}

	public async Task<List<Client>> GetAllAsync()
	{
		return await _database.Clients.Find(_ => true).ToListAsync();
	}

	public async Task<Client?> GetAsync(string id)
	{
		return await _database.Clients.Find(x => x.Id == id).FirstOrDefaultAsync();
	}

	public async Task<string> CreateAsync(Client client)
	{
		await _database.Clients.InsertOneAsync(client);
		if (client.Id == null)
		{
			throw new NullReferenceException(nameof(client.Id));
		}
		return client.Id;
	}

	public async Task ReplaceAsync(string id, Client client)
	{
		await _database.Clients.ReplaceOneAsync(x => x.Id == id, client);
	}

	public async Task<bool> DeleteAsync(string id)
	{
		var result = await _database.Clients.DeleteOneAsync(x => x.Id == id);
		return result.DeletedCount > 0;
	}
}
