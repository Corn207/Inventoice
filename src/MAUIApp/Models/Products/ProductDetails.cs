using CommunityToolkit.Mvvm.ComponentModel;

namespace MAUIApp.Models.Products;

public partial class ProductDetails : ObservableObject
{
	[ObservableProperty]
	private string _name = string.Empty;

	[ObservableProperty]
	private string _barcode = string.Empty;

	[ObservableProperty]
	private string? _group;

	[ObservableProperty]
	private string? _brand;

	[ObservableProperty]
	private string? _storagePosition;

	[ObservableProperty]
	private string? _description;

	[ObservableProperty]
	private uint _buyingPrice = 0;

	[ObservableProperty]
	private uint _sellingPrice = 0;

	public string? Id { get; init; }
	public uint? InStock { get; init; }
	public DateTime? DateCreated { get; init; }
}
