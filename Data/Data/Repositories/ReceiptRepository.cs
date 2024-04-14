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
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly TradeMarketDbContext _context;

        public ReceiptRepository(TradeMarketDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Receipt entity)
        {
            await _context.Receipts.AddAsync(entity);
        }

        public void Delete(Receipt entity)
        {
            _context.Receipts.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var receipt = await _context.Receipts.FindAsync(id);
            _context.Receipts.Remove(receipt);
        }

        public async Task<IEnumerable<Receipt>> GetAllAsync()
        {
            var receipts = await _context.Receipts.ToListAsync();

            return receipts;
        }

        public async Task<IEnumerable<Receipt>> GetAllWithDetailsAsync()
        {
            var receipts = await _context.Receipts
                .Include(r => r.ReceiptDetails)
                    .ThenInclude(rd => rd.Product)
                        .ThenInclude(p => p.Category)
                .Include(r => r.Customer)
                .ToListAsync();

            return receipts;
        }

        public async Task<Receipt> GetByIdAsync(int id)
        {
            var receipt = await _context.Receipts.FindAsync(id);

            return receipt;
        }

        public async Task<Receipt> GetByIdWithDetailsAsync(int id)
        {
            var receipt = await _context.Receipts
                .Include(r => r.ReceiptDetails)
                    .ThenInclude(rd => rd.Product)
                        .ThenInclude(p => p.Category)
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(r => r.Id == id);

            return receipt;
        }

        public void Update(Receipt entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
