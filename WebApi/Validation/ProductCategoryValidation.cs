
using Business.Models;

namespace WebApi.Validation
{
    public static class ProductCategoryValidation
    {
        public static string Validation(ProductCategoryModel model)
        {
            if(model == null)
            {
                return "Product category model is null";
            }

            if(string.IsNullOrWhiteSpace(model.CategoryName))
            {
                return "Product category model category name is empty or whitespace";
            }

            return null;
        }
    }
}
