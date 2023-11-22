namespace Domain.Entities.Interfaces;
public interface ICancellableEntity
{
	DateTime? DateCancelled { get; set; }
}
