using Domain.Entities.Interfaces;
using System.Linq.Expressions;

namespace Application.Interfaces.Repositories.Bases;
public interface IRepository<TEntity> where TEntity : IEntity
{
	Task<List<TEntity>> GetByIdsAsync(IEnumerable<string> ids);

	Task<TEntity?> GetAsync(
		string id,
		Expression<Func<TEntity, bool>>? filter = null);

	Task CreateAsync(TEntity entity);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="entity"></param>
	/// <returns></returns>
	/// <exception cref="UnknownException"></exception>
	Task ReplaceAsync(TEntity entity);

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
	Task UpdateAsync<TField>(
		string id,
		Expression<Func<TEntity, TField>> field,
		TField value,
		Expression<Func<TEntity, bool>>? filter = null);

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
	Task UpdateAsync<TField1, TField2>(
		string id,
		Expression<Func<TEntity, TField1>> field1,
		TField1 value1,
		Expression<Func<TEntity, TField2>> field2,
		TField2 value2,
		Expression<Func<TEntity, bool>>? filter = null);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	/// <exception cref="InvalidIdException"></exception>
	Task DeleteAsync(string id);
}
