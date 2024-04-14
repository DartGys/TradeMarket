using Business.Models;
using Data.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Validation
{
    public static class ProductValidation
    {
        public static void ModelValidator(ProductModel model)
        {
            if(model == null)
            {
                throw new MarketException("Product model is null");
            }

            if(string.IsNullOrWhiteSpace(model.ProductName))
            {
                throw new MarketException("Product model product name is empty");
            }

            if (model.Price < 0)
            {
                throw new MarketException("Product model price error");
            }

            if (model.ProductCategoryId == 0)
            {
                throw new MarketException("Product model product category id error");
            }
        }
    }
}
