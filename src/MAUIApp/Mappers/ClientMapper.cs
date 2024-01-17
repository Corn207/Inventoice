using Domain.DTOs.Clients;
using Domain.Entities;
using MAUIApp.Models.Clients;

namespace MAUIApp.Mappers;
public static class ClientMapper
{
	public static Details ToDetails(Client source)
	{
		if (string.IsNullOrWhiteSpace(source.Id))
		{
			throw new ArgumentException($"Id is empty while mapping {nameof(Client)} to {nameof(Details)}.");
		}

		var target = new Details()
		{
			Name = source.Name,
			Phonenumber = source.Phonenumber,
			Email = source.Email,
			Address = source.Address,
			Description = source.Description,
			Id = source.Id,
			DateCreated = source.DateCreated,
		};

		return target;
	}

	public static ClientCreateUpdate ToCreateUpdate(Details source)
	{
		var target = new ClientCreateUpdate()
		{
			Name = source.Name,
			Phonenumber = source.Phonenumber,
			Email = string.IsNullOrWhiteSpace(source.Email) ? null : source.Email,
			Address = string.IsNullOrWhiteSpace(source.Address) ? null : source.Address,
			Description = string.IsNullOrWhiteSpace(source.Description) ? null : source.Description,
		};

		return target;
	}
}
