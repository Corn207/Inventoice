using CommunityToolkit.Mvvm.ComponentModel;

namespace MAUIApp.Models.Products;
public partial class ProductItem : ObservableObject
{
	public required string Id { get; init; }
	public required string Name { get; init; }
	public required string Barcode { get; init; }
	public required uint InStock { get; init; }
	public long TotalPrice => Quantity * (long)Price;
	public long Difference => Quantity - (long)InStock;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(TotalPrice))]
	[NotifyPropertyChangedFor(nameof(Difference))]
	private uint _quantity;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(TotalPrice))]
	[NotifyPropertyChangedFor(nameof(Difference))]
	private uint _price;
}
