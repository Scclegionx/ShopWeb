﻿using ShopWeb.Models.Domain;

namespace ShopWeb.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync(int? page, int? pageSize);
        Task<Product?> GetAsync(Guid id);
        Task<Product> AddAsync(Product product);
        Task<Product?> UpdateAsync(Product product);
        Task<Product?> DeleteAsync(Guid id);
        Task<IEnumerable<Product?>> FindByNameAsync(string productName);
        Task<int> GetTotalProductsCount();
    }
}
