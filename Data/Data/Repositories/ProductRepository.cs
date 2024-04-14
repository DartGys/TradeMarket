using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly TradeMarketDbContext _context;

        public ProductRepository(TradeMarketDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product entity)
        {
            await _context.Products.AddAsync(entity);
        }

        public void Delete(Product entity)
        {
            _context.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = await _context.Products.ToListAsync();
            return products;
        }

        public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
        {
            var products = await _context.Products
                .Include(p => p.ReceiptDetails)
                    .ThenInclude(rd => rd.Receipt)
                .Include(p => p.Category)
                .ToListAsync();

            return products;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            return product;
        }

        public async Task<Product> GetByIdWithDetailsAsync(int id)
        {
            var product = await _context.Products
                 .Include(p => p.ReceiptDetails)
                     .ThenInclude(rd => rd.Receipt)
                 .Include(p => p.Category)
                 .FirstOrDefaultAsync(p => p.Id == id);

            return product;
        }

        public void Update(Product entity)
        {
            var existingProduct = _context.Products.Find(entity.Id);

            if (existingProduct != null)
            {
                _context.Entry(existingProduct).CurrentValues.SetValues(entity);
                _context.Entry(existingProduct).State = EntityState.Modified;
            }
            else
            {
                _context.Products.Add(entity);
            }
        }
    }
}
