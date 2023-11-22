using Domain.Entities.Interfaces;
using MongoDB.Driver;

namespace Infrastructure;
public class Database
{
	private readonly IMongoDatabase _database;

	public Database(IMongoDatabase database)
	{
		_database = database;
	}

	public IMongoCollection<TEntity> Collection<TEntity>() where TEntity : IEntity
	{
		return _database.GetCollection<TEntity>(typeof(TEntity).Name);
	}
}
