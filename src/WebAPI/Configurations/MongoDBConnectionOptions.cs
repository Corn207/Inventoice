namespace WebAPI.Configurations;

public class MongoDBConnectionOptions
{
	public const string SectionName = "MongoConnection";

	public string ConnectionString { get; set; } = string.Empty;
	public string DatabaseName { get; set; } = string.Empty;
}
