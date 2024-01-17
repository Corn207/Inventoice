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
	public static User ToEntity(UserCreateUpdate source)
	{
		var target = new User()
		{
			Name = source.Name,
			Phonenumber = source.Phonenumber,
			DateCreated = DateTime.UtcNow,
		};

		return target;
	}
	public static partial void ToEntity(UserCreateUpdate source, User target);
}
