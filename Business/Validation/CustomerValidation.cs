using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Validation
{
    public static class CustomerValidation
    {
        public static void ModelValidator(CustomerModel model )
        {
            if(model == null)
            {
                throw new MarketException("Customer model is null");
            }

            if(string.IsNullOrWhiteSpace(model.Name) )
            {
                throw new MarketException("Customer model Name empty");
            }

            if (string.IsNullOrWhiteSpace(model.Surname))
            {
                throw new MarketException("Customer model Surname empty");
            }

            if (model.BirthDate < new DateTime(1900, 01, 01, 0, 0, 0, DateTimeKind.Utc) || model.BirthDate > DateTime.UtcNow)
            {
                throw new MarketException("Customer model BirthDate error");
            }
        }
    }
}
