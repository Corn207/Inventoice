using Core.Entities;
using MongoDB.Driver;

namespace Infrastructure;
public class Database
{
	public Database(string connectionString, string databaseName)
	{
		var client = new MongoClient(connectionString);
		var database = client.GetDatabase(databaseName);

		AuditReports = database.GetCollection<AuditReport>("auditReports");
		Clients = database.GetCollection<Client>("clients");
		ExportReports = database.GetCollection<ExportReport>("exportReports");
		ImportReports = database.GetCollection<ImportReport>("importReports");
		Invoices = database.GetCollection<Invoice>("invoices");
		Products = database.GetCollection<Product>("products");
		Users = database.GetCollection<User>("users");
	}

	public IMongoCollection<AuditReport> AuditReports { get; }
	public IMongoCollection<Client> Clients { get; }
	public IMongoCollection<ExportReport> ExportReports { get; }
	public IMongoCollection<ImportReport> ImportReports { get; }
	public IMongoCollection<Invoice> Invoices { get; }
	public IMongoCollection<Product> Products { get; }
	public IMongoCollection<User> Users { get; }
}
