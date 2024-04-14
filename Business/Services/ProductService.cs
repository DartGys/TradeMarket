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
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task AddAsync(ProductModel model)
        {
            ProductValidation.ModelValidator(model);

            var entity = _mapper.Map<Product>(model);

            await _unitOfWork.ProductRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddCategoryAsync(ProductCategoryModel categoryModel)
        {
            ProductCategoryValidation.ModelValidator(categoryModel);

            var entity = _mapper.Map<ProductCategory>(categoryModel);

            await _unitOfWork.ProductCategoryRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            if(modelId == 0)
            {
                throw new MarketException();
            }

            await _unitOfWork.ProductRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            var enities = await _unitOfWork.ProductRepository.GetAllWithDetailsAsync();

            var products = _mapper.Map<IEnumerable<ProductModel>>(enities);

            return products;
        }

        public async Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync()
        {
            var enities = await _unitOfWork.ProductCategoryRepository.GetAllAsync();

            var productCategories = _mapper.Map<IEnumerable<ProductCategoryModel>>(enities);

            return productCategories;
        }

        public async Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch)
        {
            var query = await _unitOfWork.ProductRepository.GetAllWithDetailsAsync(); 

            if (filterSearch.CategoryId.HasValue)
            {
                query = query.Where(p => p.ProductCategoryId == filterSearch.CategoryId);
            }

            if (filterSearch.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= filterSearch.MinPrice);
            }

            if (filterSearch.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filterSearch.MaxPrice);
            }

            var productsEntity = query.ToList();

            var products = _mapper.Map<IEnumerable<ProductModel>>(productsEntity);

            return products;
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            if(id == 0)
            {
                throw new MarketException();
            }

            var entity = await _unitOfWork.ProductRepository.GetByIdWithDetailsAsync(id);
            
            var products = _mapper.Map<ProductModel>(entity);

            return products;
        }

        public async Task RemoveCategoryAsync(int categoryId)
        {
            if(categoryId == 0)
            {
                throw new MarketException();
            }

            await _unitOfWork.ProductCategoryRepository.DeleteByIdAsync(categoryId);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(ProductModel model)
        {
            ProductValidation.ModelValidator(model);

            var entity = _mapper.Map<Product>(model);

            _unitOfWork.ProductRepository.Update(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCategoryAsync(ProductCategoryModel categoryModel)
        {
            ProductCategoryValidation.ModelValidator(categoryModel);

            var entity = _mapper.Map<ProductCategory>(categoryModel);

            _unitOfWork.ProductCategoryRepository.Update(entity);
            await _unitOfWork.SaveAsync();
        }
    }
}
