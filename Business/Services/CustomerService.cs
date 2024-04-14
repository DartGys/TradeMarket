using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(CustomerModel model)
        {
            CustomerValidation.ModelValidator(model);

            var entity = _mapper.Map<Customer>(model);

            await _unitOfWork.CustomerRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            if(modelId == 0)
            {
                throw new MarketException();
            }

            await _unitOfWork.CustomerRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CustomerModel>> GetAllAsync()
        {
            var entity = await _unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
            var list = entity.ToList();
            var customers = _mapper.Map<IEnumerable<CustomerModel>>(list);

            return customers;
        }

        public async Task<CustomerModel> GetByIdAsync(int id)
        {
            if(id == 0)
            {
                throw new MarketException();
            }

            var entity = await _unitOfWork.CustomerRepository.GetByIdWithDetailsAsync(id);
            var customer = _mapper.Map<CustomerModel>(entity);

            return customer;
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
        {
            if(productId == 0)
            {
                throw new MarketException();
            }

            var customersEntity = await _unitOfWork.CustomerRepository
                .GetAllWithDetailsAsync(); 

            var customersByProductId = customersEntity
                .Where(c => c.Receipts
                    .Any(r => r.ReceiptDetails
                        .Any(rd => rd.ProductId == productId)))
                .Distinct()
                .ToList();

            var customers = _mapper.Map<IEnumerable<CustomerModel>>(customersByProductId);

            return customers;
        }

        public async Task UpdateAsync(CustomerModel model)
        {
            CustomerValidation.ModelValidator(model);

            var entity = _mapper.Map<Customer>(model);

            _unitOfWork.CustomerRepository.Update(entity);
            await _unitOfWork.SaveAsync();
        }
    }
}
