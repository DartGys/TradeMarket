using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Validation
{
    public static class ReceiptValidation
    {
        public static void ModelValidator(ReceiptModel model)
        {
            if (model == null)
            {
                throw new MarketException("Receipt model is null");
            }

            if (model.OperationDate < new DateTime(1900, 01, 01, 0, 0, 0, DateTimeKind.Utc) || model.OperationDate > DateTime.UtcNow)
            {
                throw new MarketException("Receipt model operation date error");
            }

            if (model.CustomerId == 0)
            {
                throw new MarketException("Receipt model Customer id error");
            }
        }
    }
}
