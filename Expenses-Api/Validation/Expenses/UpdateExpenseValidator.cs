using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.DAL;
using ExpensesApi.Models;
using ExpensesApi.Services.Expenses;

namespace ExpensesApi.Validation.Expenses {
  /// <summary>
  /// Handles all Update specific validation required.
  /// Should not contain any validation which would be shared across both Add
  /// and Update operations, these should go in the AddUpdateExpenseValidator
  /// </summary>
  public sealed class UpdateExpenseValidator : Validator<UpdateExpenseInfo> {
    ExpenseContext expensesDb;

    public UpdateExpenseValidator(ExpenseContext expensesDb) {
      this.expensesDb = expensesDb;
    }

    protected override IEnumerable<ValidationResult> Validate(UpdateExpenseInfo command) {
      List<ValidationResult> validationResults = new List<ValidationResult>();

      Expense existingExpense = expensesDb.Expenses
                                          .Where(e => e.expenseId == command.expenseId)
                                          .SingleOrDefault();

      // Check any IDs or foreign keys are correct
      // Should be done for *all* adds/updates to expenses
      if (existingExpense == null) {
        validationResults.Add(new ValidationResult("ID",
                                                   $"No expense exists with the ID: {command.expenseId}"));
      }

      return validationResults;
    }
  }
}
