﻿using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.DTOs.ExportReports;
using Domain.Entities;
using Domain.Mappers;

namespace Application.Services;
public class ExportReportService(
	IExportReportRepository exportReportRepository,
	IProductRepository productRepository,
	IUserRepository userRepository)
{
	public async Task<PartialArray<ExportReportShort>> SearchAsync(
		string? productNameOrBarcode,
		string? authorName,
		TimeRange timeRange,
		OrderBy orderBy,
		Pagination pagination)
	{
		var entities = await exportReportRepository.SearchAsync(
			productNameOrBarcode,
			authorName,
			timeRange,
			orderBy,
			pagination);

		return entities.ToPartialArray(ExportReportMapper.ToShort);
	}

	public async Task<uint> CountAllAsync()
	{
		var count = await exportReportRepository.CountAllAsync();

		return count;
	}

	public async Task<ExportReport?> GetAsync(string id)
	{
		return await exportReportRepository.GetAsync(id);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="authorUserId"></param>
	/// <param name="create"></param>
	/// <returns></returns>
	/// <exception cref="NotFoundException"></exception>
	/// <exception cref="OutOfStockException"></exception>
	public async Task<string> CreateAsync(string authorUserId, ExportReportCreate create)
	{
		var user = await userRepository.GetAsync(authorUserId)
			?? throw new NotFoundException("ExportReport.Author's UserId was not found.", authorUserId);
		var createIds = create.ProductItems.Select(x => x.Id).ToArray();
		var products = await productRepository.GetByIdsAsync(createIds);
		if (products.Count != create.ProductItems.Length)
		{
			var nonExistingIds = createIds.Except(products.Select(x => x.Id!));
			throw new NotFoundException("ExportReport.ProductItems' Ids were not found.", nonExistingIds.ToArray());
		}

		var changes = products
			.Join(
				create.ProductItems,
				product => product.Id,
				create => create.Id,
				(product, create) => new
				{
					create.Id,
					product.Name,
					product.Barcode,
					product.InStock,
					create.Quantity
				})
			.ToArray();

		var outOfStockProductIds = changes
			.Where(x => x.InStock < x.Quantity)
			.Select(x => x.Id)
			.ToArray();
		if (outOfStockProductIds.Length > 0)
		{
			throw new OutOfStockException(outOfStockProductIds);
		}

		#region Creating entity
		var userInfo = UserMapper.ToInfo(user);
		var items = changes
			.Select(x => new ExportReportProductItem()
			{
				Id = x.Id,
				Name = x.Name,
				Barcode = x.Barcode,
				Quantity = x.Quantity
			})
			.ToList();
		var entity = new ExportReport()
		{
			Author = userInfo,
			ProductItems = items,
			DateCreated = DateTime.UtcNow
		};
		#endregion

		var tasks = changes
			.Select(x => productRepository.UpdateAsync(x.Id, (x => x.InStock, x.InStock - x.Quantity)))
			.Append(exportReportRepository.CreateAsync(entity));
		await Task.WhenAll(tasks);

		return entity.Id!;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	/// <exception cref="NotFoundException"></exception>
	public async Task DeleteAsync(string id)
	{
		await exportReportRepository.DeleteAsync(id);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	/// <exception cref="NotFoundException"></exception>
	public async Task CancelAsync(string id)
	{
		var report = await exportReportRepository.GetAsync(id, x => x.DateCancelled == null)
			?? throw new NotFoundException("ExportReport was not found or cancelled.", id);
		var products = await productRepository.GetByIdsAsync(report.ProductItems.Select(x => x.Id));

		var tasks = products
			.Join(
				report.ProductItems,
				product => product.Id,
				report => report.Id,
				(product, report) => productRepository.UpdateAsync(report.Id, (x => x.InStock, product.InStock + report.Quantity)))
			.Append(exportReportRepository.UpdateAsync(id, (x => x.DateCancelled, DateTime.UtcNow)));

		await Task.WhenAll(tasks);
	}
}
