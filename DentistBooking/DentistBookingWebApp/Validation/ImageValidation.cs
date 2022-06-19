using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace DentistBookingWebApp.Validation
{
    public class ImageValidation : ValidationAttribute
    {
        public ImageValidation()
        {
            ErrorMessage = "Wrong format for image";
        }

        public override bool IsValid(object value)
        {
            List<string> listExtensions = new List<string>
            {
                ".jpg",
                ".png"
            };
            bool result = true;
            try
            {
                //decimal price = decimal.Parse(value.ToString());
                IFormFile file = (IFormFile)value;
                if(file != null)
                {
                    string extensions = Path.GetExtension(file.FileName);
                    if(listExtensions.Exists(p => p.Equals(extensions))) {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
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
