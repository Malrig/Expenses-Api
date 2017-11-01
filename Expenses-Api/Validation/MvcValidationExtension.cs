using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExpensesApi.Validation {
  /// <summary>
  /// Extensions to the validation code for MVC
  /// </summary>
  public static class MvcValidationExtension {
    /// <summary>
    /// Adds all the errors from a validation exception to the given model state
    /// </summary>
    public static void AddModelErrors(this ModelStateDictionary state, ValidationException exception) {
      foreach (var error in exception.ValidationErrors) {
        state.AddModelError(error.key, error.message);
      }
    }
  }
}
