using Domain.Entities.Interfaces;

namespace Application.Interfaces.Repositories.Bases;
public interface ISoftDeletableRepository<TEntity> : IRepository<TEntity> where TEntity : IEntity, ISoftDeletableEntity
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	/// <exception cref="InvalidIdException"></exception>
	/// <exception cref="UnknownException"></exception>
	Task SoftDeleteAsync(string id);
}
