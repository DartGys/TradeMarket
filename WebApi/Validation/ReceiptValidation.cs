using Business.Models;
using System;

namespace WebApi.Validation
{
    public static class ReceiptValidation
    {
        public static string Validation(ReceiptModel model)
        {
            if(model == null)
            {
                return "Receipt model is null";
            }

            if(model.OperationDate < new DateTime(1900, 01, 01, 0, 0, 0, DateTimeKind.Utc) || model.OperationDate > DateTime.Now)
            {
                return "Receipt model date error";
            }

            return null;
        }
    }
}
