namespace Domain.DTOs.Invoices;
public record struct InvoiceCreate
{
	public required string AuthorUserId { get; set; }
	public required string ClientId { get; set; }
	public InvoiceCreateProductItem[] ProductItems { get; set; }
	public uint? PaidAmount { get; set; }
}

public record struct InvoiceCreateProductItem
{
	public required string ProductId { get; set; }
	public uint Quantity { get; set; }
}
