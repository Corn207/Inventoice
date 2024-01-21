using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Identity.Domain.Entity;
public class UserToken
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }
	public required string UserId { get; set; }
	public required DateTime DateCreated { get; set; }
	public required DateTime DateExpired { get; set; }
}
