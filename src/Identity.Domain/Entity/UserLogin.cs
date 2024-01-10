using Identity.Domain.Entity.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Identity.Domain.Entity;
public class UserLogin : IEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public required string UserId { get; set; }
	public required string Username { get; set; }
	public required string Password { get; set; }
}
