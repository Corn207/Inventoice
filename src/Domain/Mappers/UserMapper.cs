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
	public static User ToEntity(UserCreate source)
	{
		var target = new User()
		{
			Name = source.Name,
			Username = source.Username,
			Password = source.Password,
			Phonenumber = source.Phonenumber,
			DateCreated = DateTime.UtcNow,
		};

		return target;
	}
	public static partial void ToEntity(UserUpdate source, User target);
}
