using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.DAL;
using ExpensesApi.Models;
using ExpensesApi.Services.ExpenseLines;

namespace ExpensesApi.Validation.ExpenseLines {
  public sealed class AddUpdateExpenseLineValidator : Validator<AddUpdateExpenseLineInfo> {
    ExpenseContext expensesDb;

    public AddUpdateExpenseLineValidator(ExpenseContext expensesDb) {
      this.expensesDb = expensesDb;
    }

    protected override IEnumerable<ValidationResult> Validate(AddUpdateExpenseLineInfo command) {
      List<ValidationResult> validationResults = new List<ValidationResult>();

      ExpenseLine existingExpenseLine = expensesDb.ExpenseLines
                                                  .Where(el => el.expenseLineId == command.expenseLineId)
                                                  .SingleOrDefault();

      Expense existingExpense = expensesDb.Expenses
                                          .Where(e => e.expenseId == command.expenseId)
                                          .SingleOrDefault();

      // Check any IDs or foreign keys are correct
      // Should be done for *all* adds/updates to expenses
      if ((command.expenseLineId != 0) &&
          (existingExpenseLine == null)) {
        validationResults.Add(new ValidationResult("ID",
                                                   $"No expense line exists with the ID: {command.expenseLineId}"));
      }
      if (existingExpense == null) {
        validationResults.Add(new ValidationResult("ID",
                                                   $"No expense exists with the ID: {command.expenseId}"));
      }
      // Check basic requirements for expenses
      if ((command.name == null) ||
          (command.name.Trim().Length == 0)) {
        validationResults.Add(new ValidationResult("Name",
                                                   "Name is required on an expense line."));
      }

      return validationResults;
    }
  }
}