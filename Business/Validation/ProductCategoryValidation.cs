using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Validation
{
    public static class ProductCategoryValidation
    {
        public static void ModelValidator(ProductCategoryModel model)
        {
            if (model == null)
            {
                throw new MarketException("ProductCategory model is null");
            }
            
            if (string.IsNullOrWhiteSpace(model.CategoryName))
            {
                throw new MarketException("ProductCategory model category name is empty");
            }
        }
    }
}
