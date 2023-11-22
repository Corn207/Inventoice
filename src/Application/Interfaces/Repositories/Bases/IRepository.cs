using Domain.Entities.Interfaces;
using System.Linq.Expressions;

namespace Application.Interfaces.Repositories.Bases;
public interface IRepository<TEntity> where TEntity : IEntity
{
	Task<List<TEntity>> GetByIdsAsync(
		IEnumerable<string> ids);
	Task<List<TProjection>> GetByIdsAsync<TProjection>(
		IEnumerable<string> ids,
		Expression<Func<TEntity, TProjection>> projection);
	Task<TEntity?> GetAsync(
		string id,
		Expression<Func<TEntity, bool>>? filter = null);
	Task<TProjection?> GetAsync<TProjection>(
		string id,
		Expression<Func<TEntity, TProjection>> projection,
		Expression<Func<TEntity, bool>>? filter = null) where TProjection : class;

	Task CreateAsync(TEntity entity);

	Task ReplaceAsync(TEntity entity);
	Task UpdateAsync<TField>(
		string id,
		Expression<Func<TEntity, TField>> field,
		TField value,
		Expression<Func<TEntity, bool>>? filter = null);
	Task UpdateAsync<TField1, TField2>(
		string id,
		Expression<Func<TEntity, TField1>> field1,
		TField1 value1,
		Expression<Func<TEntity, TField2>> field2,
		TField2 value2,
		Expression<Func<TEntity, bool>>? filter = null);
	Task UpdateAsync<TField1, TField2, TField3>(
		string id,
		Expression<Func<TEntity, TField1>> field1,
		TField1 value1,
		Expression<Func<TEntity, TField2>> field2,
		TField2 value2,
		Expression<Func<TEntity, TField3>> field3,
		TField3 value3,
		Expression<Func<TEntity, bool>>? filter = null);

	Task DeleteAsync(string id);

	Task<bool> ExistsAsync(
		IEnumerable<string> ids,
		Expression<Func<TEntity, bool>>? filter = null);
}
