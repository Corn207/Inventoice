using Domain.DTOs.Products;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Domain.Mappers;
[Mapper]
public static partial class ProductMapper
{
	public static partial ProductShort ToShortForm(Product source);
	public static partial Product ToEntity(ProductCreateUpdate source);
	public static partial void ToEntity(ProductCreateUpdate source, Product target);
}
