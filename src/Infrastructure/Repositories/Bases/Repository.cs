using Application.Exceptions;
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
	#endregion

	#region Create
	public virtual async Task CreateAsync(TEntity entity)
	{
		await Database.Collection<TEntity>().InsertOneAsync(entity);
	}
	#endregion

	#region Update
	/// <summary>
	/// 
	/// </summary>
	/// <param name="entity"></param>
	/// <returns></returns>
	/// <exception cref="UnknownException"></exception>
	public virtual async Task ReplaceAsync(TEntity entity)
	{
		var result = await Database.Collection<TEntity>()
			.ReplaceOneAsync(x => x.Id == entity.Id, entity);

		if (result.ModifiedCount == 0)
		{
			throw new UnknownException("ReplaceOne.ModifiedCount is 0.");
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TField"></typeparam>
	/// <param name="id"></param>
	/// <param name="field"></param>
	/// <param name="value"></param>
	/// <param name="filter"></param>
	/// <returns></returns>
	/// <exception cref="InvalidIdException"></exception>
	/// <exception cref="UnknownException"></exception>
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
			throw new InvalidIdException("Id was not found or already soft deleted.");
		}
		if (result.ModifiedCount == 0)
		{
			throw new UnknownException("UpdateResult.ModifiedCount is 0");
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TField1"></typeparam>
	/// <typeparam name="TField2"></typeparam>
	/// <param name="id"></param>
	/// <param name="field1"></param>
	/// <param name="value1"></param>
	/// <param name="field2"></param>
	/// <param name="value2"></param>
	/// <param name="filter"></param>
	/// <returns></returns>
	/// <exception cref="InvalidIdException"></exception>
	/// <exception cref="UnknownException"></exception>
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
			throw new InvalidIdException("Id was not found or already soft deleted.");
		}
		if (result.ModifiedCount == 0)
		{
			throw new UnknownException("UpdateResult.ModifiedCount is 0");
		}
	}
	#endregion

	#region Delete
	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	/// <exception cref="InvalidIdException"></exception>
	public virtual async Task DeleteAsync(string id)
	{
		var result = await Database.Collection<TEntity>()
			.DeleteOneAsync(x => x.Id == id);

		if (result.DeletedCount == 0)
		{
			throw new InvalidIdException("Id was not found.", [id]);
		}
	}
	#endregion
}
