using Business.Models;
using System;

namespace WebApi.Validation
{
    public static class CustomerValidation
    {
        public static string Validation(CustomerModel model)
        {
            if(model == null)
            {
                return "Customer model is null";
            }

            if(string.IsNullOrWhiteSpace(model.Name))
            {
                return "Customer model name is empty or whitespace";
            }

            if (string.IsNullOrWhiteSpace(model.Surname))
            {
                return "Customer model name is empty or whitespace";
            }

            if (model.BirthDate < new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc) || model.BirthDate > DateTime.UtcNow)
            {
                return "Customer BirthDate error";
            }

            if (model.DiscountValue < 0)
            {
                return "Customer model discount value less than 0";
            }

            return null;
        }
    }
}
