namespace WebAPI.Configurations;

public class MainMongoDBOptions
{
	public const string SectionName = "MainMongoDB";

	public string ConnectionString { get; set; } = string.Empty;
	public string DatabaseName { get; set; } = string.Empty;
}
