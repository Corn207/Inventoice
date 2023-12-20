using Domain.DTOs.Clients;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Domain.Mappers;
[Mapper]
public static partial class ClientMapper
{
	public static partial ClientShort ToShort(Client source);
	public static partial ClientInfo ToInfo(Client source);

	public static Client ToEntity(ClientCreateUpdate source)
	{
		var target = new Client()
		{
			Name = source.Name,
			Phonenumber = source.Phonenumber,
			DateCreated = DateTime.Now,
			Email = source.Email,
			Address = source.Address,
			Description = source.Description,
		};

		return target;
	}
	public static partial void ToEntity(ClientCreateUpdate source, Client target);
}
