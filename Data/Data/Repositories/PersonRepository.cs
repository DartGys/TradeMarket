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
    public class PersonRepository : IPersonRepository
    {
        private readonly TradeMarketDbContext _context;

        public PersonRepository(TradeMarketDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Person entity)
        {
            await _context.Persons.AddAsync(entity);
        }

        public void Delete(Person entity)
        {
            _context.Persons.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            _context.Persons.Remove(person);
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            var person = await _context.Persons.ToListAsync();

            return person;
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            var person = await _context.Persons.FindAsync(id);

            return person;
        }

        public void Update(Person entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
