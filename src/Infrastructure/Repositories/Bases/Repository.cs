using Application.Interfaces.Repositories.Bases;
using Domain.Entities.Interfaces;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Bases;
public abstract class Repository<TEntity>(Database database) : IRepository<TEntity> where TEntity : IEntity
{
	protected readonly Database Database = database;

	#region Get
	public virtual async Task<List<TEntity>> GetByIdsAsync(
		IEnumerable<string> ids)
	{
		var filterDefinition = Builders<TEntity>.Filter.In(x => x.Id, ids);

		var list = await Database.Collection<TEntity>()
			.Find(filterDefinition)
			.ToListAsync();

		return list;
	}

	public virtual async Task<List<TProjection>> GetByIdsAsync<TProjection>(
		IEnumerable<string> ids,
		Expression<Func<TEntity, TProjection>> projection)
	{
		var filterDefinition = Builders<TEntity>.Filter.In(x => x.Id, ids);

		var list = await Database.Collection<TEntity>()
			.Find(filterDefinition)
			.Project(projection)
			.ToListAsync();

		return list;
	}

	public virtual async Task<TEntity?> GetAsync(
		string id,
		Expression<Func<TEntity, bool>>? filter = null)
	{
		var filterPrimary = Builders<TEntity>.Filter.Eq(x => x.Id, id);

		FilterDefinition<TEntity> filterDefinition;
		if (filter == null)
		{
			filterDefinition = filterPrimary;
		}
		else
		{
			var filterExtra = Builders<TEntity>.Filter.Where(filter);
			filterDefinition = Builders<TEntity>.Filter.And(filterPrimary, filterExtra);
		}

		var entity = await Database.Collection<TEntity>()
			.Find(filterDefinition)
			.FirstOrDefaultAsync();

		return entity;
	}

	public virtual async Task<TProjection?> GetAsync<TProjection>(
		string id,
		Expression<Func<TEntity, TProjection>> projection,
		Expression<Func<TEntity, bool>>? filter = null) where TProjection : class
	{
		var filterPrimary = Builders<TEntity>.Filter.Eq(x => x.Id, id);

		FilterDefinition<TEntity> filterDefinition;
		if (filter == null)
		{
			filterDefinition = filterPrimary;
		}
		else
		{
			var filterExtra = Builders<TEntity>.Filter.Where(filter);
			filterDefinition = Builders<TEntity>.Filter.And(filterPrimary, filterExtra);
		}

		var entity = await Database.Collection<TEntity>()
			.Find(filterDefinition)
			.Project(projection)
			.FirstOrDefaultAsync();

		return entity;
	}
	#endregion

	#region Create
	public virtual async Task CreateAsync(TEntity entity)
	{
		await Database.Collection<TEntity>().InsertOneAsync(entity);
	}
	#endregion

	#region Update
	public virtual async Task ReplaceAsync(TEntity entity)
	{
		var result = await Database.Collection<TEntity>()
			.ReplaceOneAsync(x => x.Id == entity.Id, entity);

		if (result.ModifiedCount == 0)
		{
			throw new Exception("ReplaceOne.ModifiedCount is 0.");
		}
	}

	public virtual async Task UpdateAsync<TField>(
		string id,
		Expression<Func<TEntity, TField>> field,
		TField value,
		Expression<Func<TEntity, bool>>? filter = null)
	{
		var filterPrimary = Builders<TEntity>.Filter.Eq(x => x.Id, id);

		FilterDefinition<TEntity> filterDefinition;
		if (filter == null)
		{
			filterDefinition = filterPrimary;
		}
		else
		{
			var filterExtra = Builders<TEntity>.Filter.Where(filter);
			filterDefinition = Builders<TEntity>.Filter.And(filterPrimary, filterExtra);
		}

		var update = Builders<TEntity>.Update.Set(field, value);
		var result = await Database.Collection<TEntity>().UpdateOneAsync(filterDefinition, update);

		if (result.MatchedCount == 0)
		{
			throw new KeyNotFoundException("Id was not found.");
		}
		if (result.ModifiedCount == 0)
		{
			throw new Exception();
		}
	}

	public virtual async Task UpdateAsync<TField1, TField2>(
		string id,
		Expression<Func<TEntity, TField1>> field1,
		TField1 value1,
		Expression<Func<TEntity, TField2>> field2,
		TField2 value2,
		Expression<Func<TEntity, bool>>? filter = null)
	{
		var filterPrimary = Builders<TEntity>.Filter.Eq(x => x.Id, id);

		FilterDefinition<TEntity> filterDefinition;
		if (filter == null)
		{
			filterDefinition = filterPrimary;
		}
		else
		{
			var filterExtra = Builders<TEntity>.Filter.Where(filter);
			filterDefinition = Builders<TEntity>.Filter.And(filterPrimary, filterExtra);
		}

		var update = Builders<TEntity>.Update.Set(field1, value1).Set(field2, value2);
		var result = await Database.Collection<TEntity>().UpdateOneAsync(filterDefinition, update);

		if (result.MatchedCount == 0)
		{
			throw new KeyNotFoundException("Id was not found.");
		}
		if (result.ModifiedCount == 0)
		{
			throw new Exception();
		}
	}

	public virtual async Task UpdateAsync<TField1, TField2, TField3>(
		string id,
		Expression<Func<TEntity, TField1>> field1,
		TField1 value1,
		Expression<Func<TEntity, TField2>> field2,
		TField2 value2,
		Expression<Func<TEntity, TField3>> field3,
		TField3 value3,
		Expression<Func<TEntity, bool>>? filter = null)
	{
		var filterPrimary = Builders<TEntity>.Filter.Eq(x => x.Id, id);

		FilterDefinition<TEntity> filterDefinition;
		if (filter == null)
		{
			filterDefinition = filterPrimary;
		}
		else
		{
			var filterExtra = Builders<TEntity>.Filter.Where(filter);
			filterDefinition = Builders<TEntity>.Filter.And(filterPrimary, filterExtra);
		}

		var update = Builders<TEntity>.Update.Set(field1, value1).Set(field2, value2).Set(field3, value3);
		var result = await Database.Collection<TEntity>().UpdateOneAsync(filterDefinition, update);

		if (result.MatchedCount == 0)
		{
			throw new KeyNotFoundException("Id was not found.");
		}
		if (result.ModifiedCount == 0)
		{
			throw new Exception();
		}
	}
	#endregion

	#region Delete
	public virtual async Task DeleteAsync(string id)
	{
		var result = await Database.Collection<TEntity>()
			.DeleteOneAsync(x => x.Id == id);

		if (result.DeletedCount == 0)
		{
			throw new KeyNotFoundException();
		}
	}
	#endregion

	#region Others
	public virtual async Task<bool> ExistsAsync(
		IEnumerable<string> ids,
		Expression<Func<TEntity, bool>>? filter = null)
	{
		var idArray = ids.ToArray();

		var filterPrimary = Builders<TEntity>.Filter.In(x => x.Id, idArray);
		FilterDefinition<TEntity> filterDefinition;
		if (filter == null)
		{
			filterDefinition = filterPrimary;
		}
		else
		{
			var filterExtra = Builders<TEntity>.Filter.Where(filter);
			filterDefinition = Builders<TEntity>.Filter.And(filterPrimary, filterExtra);
		}

		var count = await Database.Collection<TEntity>()
			.Find(filterDefinition)
			.CountDocumentsAsync();

		return count == idArray.LongLength;
	}
	#endregion
}
