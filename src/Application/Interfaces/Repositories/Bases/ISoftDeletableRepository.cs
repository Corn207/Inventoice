using Domain.Entities.Interfaces;

namespace Application.Interfaces.Repositories.Bases;
public interface ISoftDeletableRepository<TEntity> : IRepository<TEntity> where TEntity : IEntity, ISoftDeletableEntity
{
	Task SoftDeleteAsync(string id);
}
