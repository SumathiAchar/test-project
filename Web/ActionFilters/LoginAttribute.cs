using System.ComponentModel.DataAnnotations;
using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.ActionFilters
{
    /// <summary>
    /// Checking custom required validation for username and password field
    /// </summary>
    public class LoginAttribute : ValidationAttribute
    {
        /// <summary>
        /// IsValid
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            LoginModel userLogin = (LoginModel)validationContext.ObjectInstance;
            if (userLogin != null)
            {
                if (string.IsNullOrEmpty(userLogin.UserName) || string.IsNullOrEmpty(userLogin.Password))
                {
                    return new ValidationResult("Email/Password is required.");
                }
            }
            return ValidationResult.Success;
        }
    }
}