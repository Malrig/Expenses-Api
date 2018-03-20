using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.DAL;
using ExpensesApi.Models;
using ExpensesApi.Services.Expenses;

namespace ExpensesApi.Validation.Expenses {
  /// <summary>
  /// Handles all Add specific validation required.
  /// Should not contain any validation which would be shared across both Add
  /// and Update operations, these should go in the AddUpdateExpenseValidator
  /// </summary>
  public sealed class AddExpenseValidator : Validator<AddExpenseInfo> {
    ExpenseContext expensesDb;

    /// <summary>
    /// Constructor pulls in all the required services
    /// </summary>
    /// <param name="expensesDb"></param>
    public AddExpenseValidator(ExpenseContext expensesDb) {
      this.expensesDb = expensesDb;
    }

    /// <summary>
    /// This function actually performs the validation.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    protected override IEnumerable<ValidationResult> Validate(AddExpenseInfo command) {
      List<ValidationResult> validationResults = new List<ValidationResult>();

      // Check any IDs or foreign keys are correct
      // Should be done for *all* adds/updates to expenses

      return validationResults;
    }
  }
}
