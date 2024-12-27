﻿using Ambev.DeveloperEvaluation.Common.Extensions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DefaultContext _defaultContext;

    public ProductRepository(DefaultContext defaultContext)
    {
        _defaultContext = defaultContext;
    }

    public async Task<Product> CreateAsync(Product data, CancellationToken cancellationToken = default)
    {
        await _defaultContext.Products.AddAsync(data, cancellationToken);
        await _defaultContext.SaveChangesAsync(cancellationToken);
        return data;
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _defaultContext.Products.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<Product?> UpdateAsync(Product data, CancellationToken cancellationToken = default)
    {
        _defaultContext.Products.Update(data);
        
        await _defaultContext.SaveChangesAsync(cancellationToken);
        
        return await GetByIdAsync(data.Id,cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var data = await GetByIdAsync(id, cancellationToken);
        if (data == null)
            return false;

        _defaultContext.Products.Remove(data);

        await _defaultContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public IQueryable<Product> SearchAsync(string sort)
    {
        return _defaultContext.Products
            .AsNoTracking()
            .ApplyOrdering(sort);
    }
}