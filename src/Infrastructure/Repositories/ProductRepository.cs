﻿using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Repositories;
public class ProductRepository(Database database) : Repository<Product>(database), IProductRepository
{
	public async Task<List<Product>> SearchAsync(
		string nameOrBarcode,
		OrderBy orderBy,
		Pagination pagination)
	{
		var query = Database.Collection<Product>().AsQueryable();

		if (!string.IsNullOrWhiteSpace(nameOrBarcode))
		{
			query = query.Where(p => p.Barcode.Contains(nameOrBarcode) || p.Name.Contains(nameOrBarcode));
		}

		if (orderBy == OrderBy.Ascending)
		{
			query = query.OrderBy(x => x.DateCreated);
		}
		else
		{
			query = query.OrderByDescending(x => x.DateCreated);
		}

		var entities = await query
			.Skip((pagination.PageNumber - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return entities;
	}
}
