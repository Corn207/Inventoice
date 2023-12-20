using Domain.Entities.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;
public class User : IEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }
	public required string Name { get; set; }
	public required string Phonenumber { get; set; }
	public required string Username { get; set; }
	public required string Password { get; set; }
}

public class UserInfo
{
	public required string Id { get; set; }
	public required string Name { get; set; }
}
