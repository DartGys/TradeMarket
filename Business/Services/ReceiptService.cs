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
    public class ReceiptService : IReceiptService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ReceiptService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task AddAsync(ReceiptModel model)
        {
            ReceiptValidation.ModelValidator(model);

            var entity = _mapper.Map<Receipt>(model);

            await _unitOfWork.ReceiptRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddProductAsync(int productId, int receiptId, int quantity)
        {
            if(productId == 0 || receiptId == 0 || quantity == 0)
            {
                throw new MarketException("Add Product error");
            }

            var receiptEntity = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);

            if(receiptEntity == null)
            {
                throw new MarketException("Receipt is null");
            }


            if(receiptEntity.ReceiptDetails == null || !receiptEntity.ReceiptDetails.Any(rd => rd.ProductId == productId))
            {
                var productEntity = await _unitOfWork.ProductRepository.GetByIdAsync(productId);

                if (productEntity == null)
                {
                    throw new MarketException("Product is null");
                }

                var receiptDeatilModel = new ReceiptDetailModel()
                {
                    ProductId = productId,
                    ReceiptId = receiptId,
                    Quantity = quantity,
                    UnitPrice = productEntity.Price,
                    DiscountUnitPrice = productEntity.Price * (1 - (decimal)receiptEntity.Customer.DiscountValue / 100)
                };

                var entity = _mapper.Map<ReceiptDetail>(receiptDeatilModel);

                await _unitOfWork.ReceiptDetailRepository.AddAsync(entity);
            }
            else
            {
                var receiptDetailEntity = receiptEntity.ReceiptDetails.FirstOrDefault(rd => rd.ProductId == productId);

                if (receiptDetailEntity == null)
                {
                    throw new MarketException();
                }

                receiptDetailEntity.Quantity += quantity;

                _unitOfWork.ReceiptDetailRepository.Update(receiptDetailEntity);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task CheckOutAsync(int receiptId)
        {
            if(receiptId == 0)
            {
                throw new MarketException();
            }

            var entity = await _unitOfWork.ReceiptRepository.GetByIdAsync(receiptId);
            entity.IsCheckedOut = !entity.IsCheckedOut;

            _unitOfWork.ReceiptRepository.Update(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            if (modelId == 0)
            {
                throw new MarketException();
            }

            var receiptEntity = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(modelId);
            var receiptDetailsEntity = receiptEntity.ReceiptDetails;

            await _unitOfWork.ReceiptRepository.DeleteByIdAsync(modelId);

            foreach (var receipt in receiptDetailsEntity)
            {
                _unitOfWork.ReceiptDetailRepository.Delete(receipt);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ReceiptModel>> GetAllAsync()
        {
            var entities = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

            var receipts = _mapper.Map<IEnumerable<ReceiptModel>>(entities);

            return receipts;
        }

        public async Task<ReceiptModel> GetByIdAsync(int id)
        {
            if(id == 0)
            {
                throw new MarketException();
            }

            var entity = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(id);

            var receipt = _mapper.Map<ReceiptModel>(entity);

            return receipt;
        }

        public async Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId)
        {
            if(receiptId == 0)
            {
                throw new MarketException();
            }

            var receiptEntities = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);

            var receiptDetailsEntities = receiptEntities.ReceiptDetails;

            var receiptDetails = _mapper.Map<IEnumerable<ReceiptDetailModel>>(receiptDetailsEntities);

            return receiptDetails;
        }

        public async Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var receiptsEntity = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

            var receiptByPeriod = receiptsEntity
                .Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate)
                .ToList();

            var receipts = _mapper.Map<IEnumerable<ReceiptModel>>(receiptByPeriod);

            return receipts;
        }

        public async Task RemoveProductAsync(int productId, int receiptId, int quantity)
        {
            if (productId == 0 || receiptId == 0 || quantity == 0)
            {
                throw new MarketException("Add Product error");
            }

            var receiptEntity = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);

            if (receiptEntity == null)
            {
                throw new MarketException("Receipt is null");
            }

            var receiptDetailEntity = receiptEntity.ReceiptDetails.FirstOrDefault(rd => rd.ProductId == productId);

            if(receiptDetailEntity == null)
            {
                throw new MarketException("Receipt detail is null");
            }

            receiptDetailEntity.Quantity -= quantity;

            if(receiptDetailEntity.Quantity < 0)
            {
                throw new MarketException("Receipt detail quanitity is less than 0");
            }

            if (receiptDetailEntity.Quantity == 0)
            {
                _unitOfWork.ReceiptDetailRepository.Delete(receiptDetailEntity);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task<decimal> ToPayAsync(int receiptId)
        {
            if(receiptId == 0)
            {
                throw new MarketException();
            }

            var receiptDetailEntity = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);

            decimal toPay = 0;

            foreach(var receiptDetail in  receiptDetailEntity.ReceiptDetails)
            {
                toPay += receiptDetail.DiscountUnitPrice * receiptDetail.Quantity;
            }

            return toPay;

        }

        public async Task UpdateAsync(ReceiptModel model)
        {
            ReceiptValidation.ModelValidator(model);

            var entity = _mapper.Map<Receipt>(model);

            _unitOfWork.ReceiptRepository.Update(entity);
            await _unitOfWork.SaveAsync();
        }
    }
}
