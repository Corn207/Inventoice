using Domain.Entities.Interfaces;
using MongoDB.Driver;

namespace Infrastructure;
public class Database(IMongoDatabase database)
{
	public IMongoCollection<TEntity> Collection<TEntity>() where TEntity : IEntity
	{
		return database.GetCollection<TEntity>(typeof(TEntity).Name);
	}
}
