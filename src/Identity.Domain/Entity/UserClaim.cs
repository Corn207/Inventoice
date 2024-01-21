using MongoDB.Bson.Serialization.Attributes;

namespace Identity.Domain.Entity;
public class UserClaim
{
	[BsonId]
	public required string UserId { get; set; }
	public required Role Roles { get; set; }
}

[Flags]
public enum Role : byte
{
	Admin = 1,
	Manager = 2,
	Employee = 4,
}
