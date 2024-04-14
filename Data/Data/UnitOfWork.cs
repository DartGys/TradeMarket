using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Data.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TradeMarketDbContext _context;

        public UnitOfWork(TradeMarketDbContext context, ICustomerRepository customerRepository,
            IPersonRepository personRepository, IProductRepository productRepository,
            IProductCategoryRepository productCategoryRepository, IReceiptRepository receiptRepository,
            IReceiptDetailRepository receiptDetailRepository)
        {
            _context = context;
            CustomerRepository = customerRepository;
            PersonRepository = personRepository;
            ProductRepository = productRepository;
            ProductCategoryRepository = productCategoryRepository;
            ReceiptRepository = receiptRepository;
            ReceiptDetailRepository = receiptDetailRepository;
        }

        public ICustomerRepository CustomerRepository { get; }

        public IPersonRepository PersonRepository { get; }

        public IProductRepository ProductRepository { get; }

        public IProductCategoryRepository ProductCategoryRepository { get; }

        public IReceiptRepository ReceiptRepository { get; }

        public IReceiptDetailRepository ReceiptDetailRepository { get; }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
