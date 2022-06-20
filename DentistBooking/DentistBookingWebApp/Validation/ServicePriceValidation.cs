using System;
using System.ComponentModel.DataAnnotations;

namespace DentistBookingWebApp.Validation
{
    public class ServicePriceValidation : ValidationAttribute
    {
        public ServicePriceValidation()
        {
            ErrorMessage = "Wrong format for service's price";
        }

        public override bool IsValid(object value)
        {
            bool result = true;
            try
            {
                decimal price = decimal.Parse(value.ToString());
                if(price < 0 && price > 1000)
                {
                    ErrorMessage = "Range for service price should be between 0 - 10000";
                    return false;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
    }
}
