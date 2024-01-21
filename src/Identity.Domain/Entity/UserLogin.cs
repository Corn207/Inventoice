using MongoDB.Bson.Serialization.Attributes;

namespace Identity.Domain.Entity;
public class UserLogin
{
	[BsonId]
	public required string UserId { get; set; }
	public required string Username { get; set; }
	public required string Password { get; set; }
}
