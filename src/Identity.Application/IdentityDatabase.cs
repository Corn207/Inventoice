using Identity.Domain.Entity;
using MongoDB.Driver;

namespace Identity.Application;
public class IdentityDatabase(IMongoDatabase database)
{
	public IMongoCollection<UserLogin> UserLogins { get; } = database.GetCollection<UserLogin>("UserLogins");
	public IMongoCollection<UserClaim> UserClaims { get; } = database.GetCollection<UserClaim>("UserClaims");
	public IMongoCollection<UserToken> UserTokens { get; } = database.GetCollection<UserToken>("UserTokens");
}
