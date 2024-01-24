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
		var filter = Builders<TEntity>.Filter.In(x => x.Id, ids);

		var list = await Database.Collection<TEntity>()
			.Find(filter)
			.ToListAsync();

		return list;
	}

	public virtual async Task<TEntity?> GetAsync(
		string id,
		params Expression<Func<TEntity, bool>>[] filters)
	{
		var all = filters
			.Select(x => Builders<TEntity>.Filter.Where(x))
			.Append(Builders<TEntity>.Filter.Eq(x => x.Id, id));
		var filter = Builders<TEntity>.Filter.And(all);

		var entity = await Database.Collection<TEntity>()
			.Find(filter)
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

		if (result.MatchedCount == 0)
		{
			throw new NotFoundException();
		}
	}

	public virtual async Task UpdateAsync<TField>(
		string id,
		params (Expression<Func<TEntity, TField>> selector, TField value)[] sets)
	{
		var filter = Builders<TEntity>.Filter.Eq(x => x.Id, id);
		var updates = sets.Select(x => Builders<TEntity>.Update.Set(x.selector, x.value));
		var update = Builders<TEntity>.Update.Combine(updates);

		var result = await Database.Collection<TEntity>().UpdateOneAsync(filter, update);

		if (result.MatchedCount == 0)
		{
			throw new NotFoundException();
		}
	}

	public virtual async Task UpdateAsync<TField>(
		IEnumerable<Expression<Func<TEntity, bool>>> filters,
		params (Expression<Func<TEntity, TField>> selector, TField value)[] sets)
	{
		var all = filters.Select(x => Builders<TEntity>.Filter.Where(x));
		var filter = Builders<TEntity>.Filter.And(all);
		var updates = sets.Select(x => Builders<TEntity>.Update.Set(x.selector, x.value));
		var update = Builders<TEntity>.Update.Combine(updates);

		var result = await Database.Collection<TEntity>().UpdateManyAsync(filter, update);

		if (result.MatchedCount == 0)
		{
			throw new NotFoundException();
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
			throw new NotFoundException();
		}
	}
	#endregion

	#region Count
	public virtual async Task<uint> CountAllAsync()
	{
		var count = await Database.Collection<TEntity>()
			.EstimatedDocumentCountAsync();

		return Convert.ToUInt32(count);
	}
	#endregion
}
