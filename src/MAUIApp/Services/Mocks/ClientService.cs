using Domain.DTOs;
using Domain.DTOs.Clients;
using Domain.Entities;
using Domain.Mappers;
using MAUIApp.Services.Interfaces;

namespace MAUIApp.Services.Mocks;
public class ClientService : IClientService
{
	private readonly Client[] _entities =
	[
		new Client()
		{
			Id = "13ssa",
			Name = "Nguyễn Văn A",
			Phonenumber = "1234567890",
			Address = "Hà Nội",
			DateCreated = DateTime.Now,
			Description = "Description",
			Email = "test@gmail.com"
		},

		new Client()
		{
			Id = "102nsav",
			Name = "Tạ Đức Hiển",
			Phonenumber = "08431246875",
			DateCreated = DateTime.Now,
		},

		new Client()
		{
			Id = "dmbl193",
			Name = "Lê Văn Thuận",
			Phonenumber = "07516892451",
			DateCreated = DateTime.Now,
			Email = "test112@gmail.com",
		}
	];

	public async Task<ClientShort[]> SearchAsync(
		string? nameOrPhonenumber = null,
		OrderBy orderBy = OrderBy.Ascending,
		ushort pageNumber = 1,
		ushort pageSize = 15)
	{
		var results = _entities.Select(ClientMapper.ToShort).ToArray();

		return await Task.FromResult(results);
	}

	public async Task<Client> GetAsync(string id)
	{
		var result = _entities.First(x => x.Id == id);
		await Task.Delay(1500);
		return await Task.FromResult(result);
	}

	public Task CreateAsync(ClientCreateUpdate data)
	{
		return Task.CompletedTask;
	}

	public Task UpdateAsync(ClientCreateUpdate data)
	{
		return Task.CompletedTask;
	}

	public Task DeleteAsync(string id)
	{
		return Task.CompletedTask;
	}
}
