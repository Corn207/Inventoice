using Domain.DTOs.Clients;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Domain.Mappers;
[Mapper]
public static partial class ClientMapper
{
	public static partial ClientShort ToShortForm(Client source);
	public static partial Client ToEntity(ClientCreateUpdate source);
	public static partial void ToEntity(ClientCreateUpdate source, Client target);
}
