namespace Domain.DTOs.Invoices;
public record struct InvoiceShort
{
	public required string Id { get; set; }
	public InvoiceStatus Status { get; set; }
	public string? ClientName { get; set; }
	public DateTime DateCreated { get; set; }
	public uint PaidAmount { get; set; }
	public ushort TotalProduct { get; set; }
	public string FirstProductName { get; set; }
	public uint FirstProductQuantity { get; set; }
}
