using Business.Models;

namespace WebApi.Validation
{
    public static class ProductValidation
    {
        public static string Validation(ProductModel model)
        {
            if (model == null)
            {
                return "Product model is null";
            }

            if(string.IsNullOrWhiteSpace(model.ProductName))
            {
                return "Product model product name is empty or whitespace";
            }

            if(model.Price < 0)
            {
                return "Product model is less than zero";
            }

            return null;
        }
    }
}
