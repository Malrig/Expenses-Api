using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.DAL;
using ExpensesApi.Models;
using ExpensesApi.Services.Expenses;

namespace ExpensesApi.Validation.Expenses {
  /// <summary>
  /// Handles all validation rules which are shared by
  /// both the Add and Update operations for an expense.
  /// 
  /// Any specific Add or Update validation required should
  /// go into the specific Add/UpdateExpenseValidators.
  /// </summary>
  public sealed class AddUpdateExpenseValidator : Validator<AddUpdateExpenseInfo> {
    ExpenseContext expensesDb;

    /// <summary>
    /// Constructor pulls in all the required services
    /// </summary>
    /// <param name="expensesDb"></param>
    public AddUpdateExpenseValidator(ExpenseContext expensesDb) {
      this.expensesDb = expensesDb;
    }

    /// <summary>
    /// This function actually performs the validation.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    protected override IEnumerable<ValidationResult> Validate(AddUpdateExpenseInfo command) {
      List<ValidationResult> validationResults = new List<ValidationResult>();

      Expense existingExpense = expensesDb.Expenses
                                          .Where(e => e.expenseId == command.expenseId)
                                          .SingleOrDefault();

      // Check any IDs or foreign keys are correct
      // Should be done for *all* adds/updates to expenses
      if ((command.expenseId != 0) &&
          (existingExpense == null)) {
        validationResults.Add(new ValidationResult("ID",
                                                   $"No expense exists with the ID: {command.expenseId}"));
      }
      // Check basic requirements for expenses
      if ((command.name == null) ||
          (command.name.Trim().Length == 0)) {
        validationResults.Add(new ValidationResult("Name",
                                                   "Name is required on an expense item"));
      }
      if ((command.billedDate == null) ||
          (command.billedDate == DateTime.MinValue)) {
        validationResults.Add(new ValidationResult("BilledDate",
                                                   "A billed date is required for an expense item"));
      }

      return validationResults;
    }
  }
}
