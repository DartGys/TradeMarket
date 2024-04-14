using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public StatisticService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProductModel>> GetCustomersMostPopularProductsAsync(int productCount, int customerId)
        {
            if(productCount == 0 || customerId == 0)
            {
                throw new MarketException();
            }

            var receiptEntity = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

            var receiptEntityByCustomer = receiptEntity.Where(r => r.CustomerId == customerId);

            var receiptDetailEntities = receiptEntityByCustomer
                .SelectMany(r => r.ReceiptDetails)
                .ToList();

            var proudctEntities = receiptDetailEntities
                .OrderByDescending(rd => rd.Quantity)
                .Take(productCount)
                .Select(rd => rd.Product)
                .ToList();

            var products = _mapper.Map<IEnumerable<ProductModel>>(proudctEntities);

            return products;
        }

        public async Task<decimal> GetIncomeOfCategoryInPeriod(int categoryId, DateTime startDate, DateTime endDate)
        {
            if (categoryId == 0 || startDate < new DateTime(1900, 01, 01, 0, 0, 0, DateTimeKind.Utc) || endDate > DateTime.UtcNow)
            {
                throw new MarketException();
            }

            var receiptEntities = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

            var receiptDetailsEntities = receiptEntities
                .Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate)
                .SelectMany(r => r.ReceiptDetails)
                .Where(p => p.Product.ProductCategoryId == categoryId)
                .ToList();

            decimal incomeOfCategory = 0;

            foreach (var item in receiptDetailsEntities)
            {
                incomeOfCategory += item.DiscountUnitPrice * item.Quantity;
            }

            return incomeOfCategory;
        }

        public async Task<IEnumerable<ProductModel>> GetMostPopularProductsAsync(int productCount)
        {
            if (productCount == 0)
            {
                throw new MarketException();
            }

            var receiptDetailEntities = await _unitOfWork.ReceiptDetailRepository.GetAllWithDetailsAsync();

            var proudctEntities = receiptDetailEntities
                .OrderByDescending(rd => rd.Quantity)
                .Take(productCount)
                .Select(rd => rd.Product)
                .ToList();

            var products = _mapper.Map<IEnumerable<ProductModel>>(proudctEntities);

            return products;
        }

        public async Task<IEnumerable<CustomerActivityModel>> GetMostValuableCustomersAsync(int customerCount, DateTime startDate, DateTime endDate)
        {
            if (customerCount == 0 || startDate < new DateTime(1900, 01, 01, 0, 0, 0, DateTimeKind.Utc) || endDate > DateTime.UtcNow)
            {
                throw new MarketException();
            }

            var receiptEntities = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

            var groupedEntities = receiptEntities
                .Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate)
                .GroupBy(r => r.CustomerId)
                .ToList();

            var combinedEntities = groupedEntities
                .SelectMany(group => group
                    .OrderByDescending(r => r.ReceiptDetails.Max(rd => rd.DiscountUnitPrice))
                    .SelectMany(r => r.ReceiptDetails.Select(rd => new { Customer = r.Customer, ReceiptDetail = rd }))
                )
                .ToList();

            var сustomers = combinedEntities
                .GroupBy(c => c.Customer.Id)
                .Take(customerCount)
                .Select(group => new CustomerActivityModel
                {
                    CustomerId = group.Key,
                    CustomerName = $"{group.First().Customer.Person.Name} {group.First().Customer.Person.Surname}",
                    ReceiptSum = group.Sum(item => item.ReceiptDetail.DiscountUnitPrice * item.ReceiptDetail.Quantity)
                })
                .ToList();

            return сustomers;

        }
    }
}
