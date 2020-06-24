using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace eShopping.Infrastructure
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //At the moment we don't need it because we don't store the file itself to the DB
            //var context = (EShoppingContext)validationContext.GetService(typeof(EShoppingContext));

            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                string[] extensions = { ".jpg", ".jpeg", ".png" };
                bool result = extensions.Any(e => string.Compare(e, extension, true) == 0);//ignoreCase

                if (!result)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return "Allowed extensions are .jpg, .jpeg and .png";
        }
    }
}
