using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly TradeMarketDbContext _context;

        public ProductCategoryRepository(TradeMarketDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ProductCategory entity)
        {
            await _context.ProductCategories.AddAsync(entity);
        }

        public void Delete(ProductCategory entity)
        {
            _context.ProductCategories.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var productCategory = await _context.ProductCategories.FindAsync(id);
            _context.ProductCategories.Remove(productCategory);
        }

        public async Task<IEnumerable<ProductCategory>> GetAllAsync()
        {
            var productCategories = await _context.ProductCategories.ToListAsync();

            return productCategories;
        }

        public async Task<ProductCategory> GetByIdAsync(int id)
        {
            var productCategory = await _context.ProductCategories.FindAsync(id);

            return productCategory;
        }

        public void Update(ProductCategory entity)
        {
            var existingProductCategory = _context.ProductCategories.Find(entity.Id);

            if (existingProductCategory != null)
            {
                _context.Entry(existingProductCategory).CurrentValues.SetValues(entity);
                _context.Entry(existingProductCategory).State = EntityState.Modified;
            }
            else
            {
                _context.ProductCategories.Add(existingProductCategory);
            }
        }
    }
}
