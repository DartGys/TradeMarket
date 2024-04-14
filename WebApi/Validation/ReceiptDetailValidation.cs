using Business.Models;

namespace WebApi.Validation
{
    public static class ReceiptDetailValidation
    {
        public static string Validation(ReceiptDetailModel model)
        {
            if(model == null)
            {
                return "Receipt detail model is null";
            }

            if(model.UnitPrice < 0 )
            {
                return "Receipt detail model unit price less than 0";
            }

            if (model.DiscountUnitPrice < 0)
            {
                return "Receipt detail model discount unit price less than 0";
            }

            if(model.Quantity < 0 )
            {
                return "Receipt detail model quantity is less than 0";
            }

            return null;
        }
    }
}
