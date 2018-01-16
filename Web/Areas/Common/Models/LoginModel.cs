using System.ComponentModel.DataAnnotations;
using SSI.ContractManagement.Web.ActionFilters;

namespace SSI.ContractManagement.Web.Areas.Common.Models
{
    public class LoginModel
    {
        [Login]
        [Display(Name = "Email")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }     
    
}