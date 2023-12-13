using Application.Interfaces.Repositories.Bases;
using Domain.Entities.Interfaces;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Bases;
public abstract class SoftDeletableRepository<TEntity>(Database database) :
	Repository<TEntity>(database),
	ISoftDeletableRepository<TEntity>
	where TEntity : IEntity, ISoftDeletableEntity
{

	#region Get
	public override async Task<List<TEntity>> GetByIdsAsync(
		IEnumerable<string> ids)
	{
		var containsIdFilterDefinition = Builders<TEntity>.Filter.In(x => x.Id, ids);
		var nonDeletedFilterDefinition = Builders<TEntity>.Filter.Where(x => x.DateDeleted == null);
		var filterDefinition = Builders<TEntity>.Filter.And(nonDeletedFilterDefinition, containsIdFilterDefinition);

		var list = await Database.Collection<TEntity>()
			.Find(filterDefinition)
			.ToListAsync();

		return list;
	}

	public override async Task<List<TProjection>> GetByIdsAsync<TProjection>(
		IEnumerable<string> ids,
		Expression<Func<TEntity, TProjection>> projection)
	{
		var containsIdFilterDefinition = Builders<TEntity>.Filter.In(x => x.Id, ids);
		var nonDeletedFilterDefinition = Builders<TEntity>.Filter.Where(x => x.DateDeleted == null);
		var filterDefinition = Builders<TEntity>.Filter.And(nonDeletedFilterDefinition, containsIdFilterDefinition);

		var list = await Database.Collection<TEntity>()
			.Find(filterDefinition)
			.Project(projection)
			.ToListAsync();

		return list;
	}

	public override async Task<TEntity?> GetAsync(
		string id,
		Expression<Func<TEntity, bool>>? filter = null)
	{
		var filterPrimary = Builders<TEntity>.Filter.Where(x => x.Id == id && x.DateDeleted == null);

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

	public override async Task<TProjection?> GetAsync<TProjection>(
		string id,
		Expression<Func<TEntity, TProjection>> projection,
		Expression<Func<TEntity, bool>>? filter = null) where TProjection : class
	{
		var filterPrimary = Builders<TEntity>.Filter.Where(x => x.Id == id && x.DateDeleted == null);

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

	#region Update
	public override async Task ReplaceAsync(TEntity entity)
	{
		var result = await Database.Collection<TEntity>()
			.ReplaceOneAsync(x => x.Id == entity.Id && x.DateDeleted == null, entity);

		if (result.ModifiedCount == 0)
		{
			throw new Exception("ReplaceOne.ModifiedCount is 0.");
		}
	}

	public override async Task UpdateAsync<TField>(
		string id,
		Expression<Func<TEntity, TField>> field,
		TField value,
		Expression<Func<TEntity, bool>>? filter = null)
	{
		var filterPrimary = Builders<TEntity>.Filter.Where(x => x.Id == id && x.DateDeleted == null);

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

	public override async Task UpdateAsync<TField1, TField2>(
		string id,
		Expression<Func<TEntity, TField1>> field1,
		TField1 value1,
		Expression<Func<TEntity, TField2>> field2,
		TField2 value2,
		Expression<Func<TEntity, bool>>? filter = null)
	{
		var filterPrimary = Builders<TEntity>.Filter.Where(x => x.Id == id && x.DateDeleted == null);

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

	public override async Task UpdateAsync<TField1, TField2, TField3>(
		string id,
		Expression<Func<TEntity, TField1>> field1,
		TField1 value1,
		Expression<Func<TEntity, TField2>> field2,
		TField2 value2,
		Expression<Func<TEntity, TField3>> field3,
		TField3 value3,
		Expression<Func<TEntity, bool>>? filter = null)
	{
		var filterPrimary = Builders<TEntity>.Filter.Where(x => x.Id == id && x.DateDeleted == null);

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
	public virtual async Task SoftDeleteAsync(string id)
	{
		var update = Builders<TEntity>.Update.Set(x => x.DateDeleted, DateTime.Now);
		var result = await Database.Collection<TEntity>()
			.UpdateOneAsync(x => x.Id == id && x.DateDeleted == null, update);

		if (result.MatchedCount == 0)
		{
			throw new KeyNotFoundException("Id was not found or already soft deleted.");
		}

		if (result.ModifiedCount == 0)
		{
			throw new Exception("UpdateResult.ModifiedCount is 0");
		}
	}
	#endregion

	#region Others
	public override async Task<bool> ExistsAsync(
		IEnumerable<string> ids,
		Expression<Func<TEntity, bool>>? filter = null)
	{
		var idArray = ids.ToArray();

		var filterIn = Builders<TEntity>.Filter.In(x => x.Id, idArray);
		var filterNonDelete = Builders<TEntity>.Filter.Where(x => x.DateDeleted == null);

		FilterDefinition<TEntity> filterDefinition;
		if (filter == null)
		{
			filterDefinition = Builders<TEntity>.Filter.And(filterIn, filterNonDelete);
		}
		else
		{
			var filterExtra = Builders<TEntity>.Filter.Where(filter);
			filterDefinition = Builders<TEntity>.Filter.And(filterIn, filterNonDelete, filterExtra);
		}

		var count = await Database.Collection<TEntity>()
			.Find(filterDefinition)
			.CountDocumentsAsync();

		return count == idArray.LongLength;
	}
	#endregion
}
