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
    public class CustomerRepository : ICustomerRepository
    {
        private readonly TradeMarketDbContext _context;
        public CustomerRepository(TradeMarketDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Customer entity)
        {
            await _context.Customers.AddAsync(entity);
        }

        public void Delete(Customer entity)
        {
            _context.Customers.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var customers = await _context.Customers.ToListAsync();

            return customers;
        }

        public async Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
        {
            var customer = await _context.Customers
                .Include(c => c.Receipts)
                    .ThenInclude(r => r.ReceiptDetails)
                .Include(c => c.Person)
                .ToListAsync();

            return customer;
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            return customer;
        }

        public async Task<Customer> GetByIdWithDetailsAsync(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Receipts)
                    .ThenInclude(c => c.ReceiptDetails)
                .Include(c => c.Person)
                .FirstOrDefaultAsync(c => c.Id == id);

            return customer;
        }

        public void Update(Customer entity)
        {
            var existingСustomer = _context.Customers.Find(entity.Id);

            if (existingСustomer != null)
            {
                entity.PersonId = existingСustomer.PersonId;
                _context.Entry(existingСustomer).CurrentValues.SetValues(entity);
                _context.Entry(existingСustomer).State = EntityState.Modified;

               
            }
            else
            {
                _context.Customers.Add(entity);
            }
        }
    }
}
