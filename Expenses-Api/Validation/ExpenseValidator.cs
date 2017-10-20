using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.Models;

namespace ExpensesApi.Validation {
  public sealed class ExpenseValidator : Validator<Expense> {
    protected override IEnumerable<ValidationResult> PerformValidation(Expense entity) {
      if ((entity.name == null) ||
          (entity.name.Trim().Length == 0)) {
        yield return new ValidationResult("Name",
                                          "Name is required.");
      }

      if ((entity.billedDate == null) ||
          (entity.billedDate == DateTime.MinValue)) {
        yield return new ValidationResult("billedDate",
                                          "A billed date is required.");
      }

      if ((entity.expenseLines == null) ||
          (entity.expenseLines.Count == 0)) {
        yield return new ValidationResult("expenseLines",
                                          "An expense must have at least one expense line.");
      }
    }
  }
}
