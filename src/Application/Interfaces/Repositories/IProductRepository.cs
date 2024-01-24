﻿using Application.Interfaces.Repositories.Bases;
using Domain.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IProductRepository : IRepository<Product>
{
	Task<PartialEnumerable<Product>> SearchAsync(
		string? nameOrBarcode,
		OrderBy orderBy,
		Pagination pagination);
}
