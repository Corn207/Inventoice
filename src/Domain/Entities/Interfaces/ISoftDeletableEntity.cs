namespace Domain.Entities.Interfaces;
public interface ISoftDeletableEntity
{
	DateTime? DateDeleted { get; set; }
}
