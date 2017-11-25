using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ExpensesApi.Identity.Models {
  // TODO review if this should be a view model and 
  // think about names etc and whether annotations are necessary.
  public class AccountRegisterLogin {
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string password { get; set; }
  }
}
