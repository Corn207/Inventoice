using Core.Entities;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs.Clients;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
	private readonly ClientRepository _clientRepository;

	public ClientsController(ClientRepository clientRepository)
	{
		_clientRepository = clientRepository;
	}

	// GET: api/<ClientsController>
	[HttpGet]
	public async Task<IEnumerable<Client>> Get()
	{
		var clients = await _clientRepository.GetAllAsync();
		return clients;
	}

	// GET api/<ClientsController>/5
	[HttpGet("{id}")]
	public async Task<ActionResult<Client>> Get(string id)
	{
		var client = await _clientRepository.GetAsync(id);
		if (client is null)
		{
			return NotFound();
		}

		return client;
	}

	// POST api/<ClientsController>
	[HttpPost]
	public async Task<IActionResult> Post([FromBody] CreateRequest body)
	{
		var client = new Client()
		{
			Name = body.Name,
			PhoneNumber = body.PhoneNumber,
			Description = body.Description,
			Email = body.Email,
			Address = body.Address,
			Gender = body.Gender,
		};

		var newId = await _clientRepository.CreateAsync(client);
		return CreatedAtAction(nameof(Get), new { id = newId }, client);
	}

	// PUT api/<ClientsController>/5
	[HttpPut("{id}")]
	public void Put(int id, [FromBody] string value)
	{
	}

	// DELETE api/<ClientsController>/5
	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(string id)
	{
		var result = await _clientRepository.DeleteAsync(id);
		if (result == true)
		{
			return NoContent();
		}
		return NotFound();
	}
}
