using Identity.Domain.Entity.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Identity.Domain.Entity;
public class UserClaim : IEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public required string UserId { get; set; }
	public required Role[] Roles { get; set; }
}

public enum Role
{
	Admin,
	Manager,
	Employee
}
