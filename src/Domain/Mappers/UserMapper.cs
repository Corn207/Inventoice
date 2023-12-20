using Domain.DTOs.Users;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Domain.Mappers;
[Mapper]
public static partial class UserMapper
{
	public static partial UserShort ToShort(User source);
	public static partial UserShort ToShort(UserInfo source);
	public static partial UserInfo ToInfo(User source);
	public static partial User ToEntity(UserCreate source);
	public static partial void ToEntity(UserUpdate source, User target);
}
