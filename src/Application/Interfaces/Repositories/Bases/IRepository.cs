using Domain.Entities.Interfaces;
using System.Linq.Expressions;

namespace Application.Interfaces.Repositories.Bases;
public interface IRepository<TEntity> where TEntity : IEntity
{
	Task<List<TEntity>> GetByIdsAsync(
		IEnumerable<string> ids);

	Task<TEntity?> GetAsync(
		string id,
		params Expression<Func<TEntity, bool>>[] filters);

	Task CreateAsync(TEntity entity);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="entity"></param>
	/// <returns></returns>
	/// <exception cref="NotFoundException"></exception>
	Task ReplaceAsync(TEntity entity);

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TField"></typeparam>
	/// <param name="id"></param>
	/// <param name="sets"></param>
	/// <returns></returns>
	/// <exception cref="NotFoundException"></exception>
	Task UpdateAsync<TField>(
		string id,
		params (Expression<Func<TEntity, TField>> selector, TField value)[] sets);

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TField"></typeparam>
	/// <param name="filters"></param>
	/// <param name="sets"></param>
	/// <returns></returns>
	/// <exception cref="NotFoundException"></exception>
	Task UpdateAsync<TField>(
		IEnumerable<Expression<Func<TEntity, bool>>> filters,
		params (Expression<Func<TEntity, TField>> selector, TField value)[] sets);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	/// <exception cref="NotFoundException"></exception>
	Task DeleteAsync(string id);

	Task<uint> CountAllAsync();
}
