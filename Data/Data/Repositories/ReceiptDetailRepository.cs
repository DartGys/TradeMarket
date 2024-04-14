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
    public class ReceiptDetailRepository : IReceiptDetailRepository
    {
        private readonly TradeMarketDbContext _context;

        public ReceiptDetailRepository(TradeMarketDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ReceiptDetail entity)
        {
            await _context.ReceiptsDetails.AddAsync(entity);
        }

        public void Delete(ReceiptDetail entity)
        {
            _context.ReceiptsDetails.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var receiptDetail = await _context.ReceiptsDetails.FindAsync(id);
            _context.ReceiptsDetails.Remove(receiptDetail);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ReceiptDetail>> GetAllAsync()
        {
            var receiptDetails = await _context.ReceiptsDetails.ToListAsync();

            return receiptDetails;
        }

        public async Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
        {
           var receiptDetails = await _context.ReceiptsDetails
                .Include(rd => rd.Product)
                    .ThenInclude(p => p.Category)
                .Include(rd => rd.Receipt)
                .ToListAsync();

            return receiptDetails;
        }

        public async Task<ReceiptDetail> GetByIdAsync(int id)
        {
            var receiptDetail = await _context.ReceiptsDetails.FindAsync(id);

            return receiptDetail;
        }

        public void Update(ReceiptDetail entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
