namespace WebAPI.Configurations;

public class IdentityMongoDBOptions
{
	public const string SectionName = "IdentityMongoDB";

	public string ConnectionString { get; set; } = string.Empty;
	public string DatabaseName { get; set; } = string.Empty;
}
