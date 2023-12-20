using Domain.Entities.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;
public class AuditReport : IEntity, ISoftDeletableEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }
	public required UserInfo Author { get; set; }
	public required List<AuditReportProductItem> ProductItems { get; set; }
	public required DateTime DateCreated { get; set; }
	public DateTime? DateDeleted { get; set; }
}

public class AuditReportProductItem
{
	public required string Id { get; set; }
	public required string Name { get; set; }
	public required string Barcode { get; set; }
	public required uint OriginalQuantity { get; set; }
	public required uint AdjustedQuantity { get; set; }
}
