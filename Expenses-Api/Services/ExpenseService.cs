using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

using ExpensesApi.Models;
using ExpensesApi.ViewModels;
using ExpensesApi.DAL;
using ExpensesApi.Validation;

namespace ExpensesApi.Services {
  // Define the Interface which can be used to interact with Expenses
  public interface IExpenseService {
    IEnumerable<Expense> ListExpenses();
    Expense GetExpense(int expenseId);
    void CreateExpense(Expense expenseToCreate);
    void UpdateExpense(int expenseId, Expense newExpenseValues);
    void DeleteExpense(int expenseId);
  }

  //// TODO - Add logging to this to capture error messages etc.
  //public class ExpenseService : IExpenseService {
  //  private ExpenseContext expenseDb;
  //  private IValidator expenseValidator;
  //  private IValidator expenseLineValidator;

  //  public ExpenseService(ExpenseContext expenseDb) {
  //    this.expenseDb = expenseDb;
  //    this.expenseValidator = new ExpenseValidator();
  //    this.expenseLineValidator = new ExpenseLineValidator();
  //  }

  //  public IEnumerable<Expense> ListExpenses() {
  //    // TODO - Return this as a ViewModel instead of the direct models.
  //    return expenseDb.Expenses
  //                    .Include(e => e.expenseLines);
  //  }

  //  public Expense GetExpense(int expenseId) {
  //    // Database logic
  //    Expense expenseToReturn = expenseDb.Expenses.Where(e => e.expenseId == expenseId)
  //                                                .Include(e => e.expenseLines)
  //                                                .SingleOrDefault();

  //    if (expenseToReturn == null) {
  //      throw new KeyNotFoundException("No expense with ID: " + expenseId);
  //    }

  //    // TODO - Return the expense as a viewmodel not hte model directly
  //    return expenseToReturn;
  //  }

  //  public void CreateExpense(Expense expenseToCreate) {
  //    // Validate the expense (this throws an error if it fails)
  //    this.expenseValidator.Validate(expenseToCreate);
  //    this.expenseLineValidator.ValidateAll(expenseToCreate.expenseLines);

  //    // Database logic
  //    expenseDb.Expenses.Add(expenseToCreate);
  //    expenseDb.SaveChanges();
  //  }

  //  public void UpdateExpense(int expenseId, Expense expenseToUpdate) {
  //    // Validate the expense (this throws an error if it fails)
  //    this.expenseValidator.Validate(expenseToUpdate);
  //    this.expenseLineValidator.ValidateAll(expenseToUpdate.expenseLines);

  //    // Database logic
  //    Expense existingExpense = expenseDb.Expenses.Where(e => e.expenseId == expenseId)
  //                                                .Include(e => e.expenseLines)
  //                                                .SingleOrDefault();

  //    if (existingExpense == null) {
  //      throw new KeyNotFoundException("No expense with ID: " + expenseId);
  //    }
  //    // Update the expense
  //    expenseDb.Entry(existingExpense).CurrentValues.SetValues(expenseToUpdate);

  //    // Delete any expense lines which are no longer required
  //    foreach (ExpenseLine existingLine in existingExpense.expenseLines) {
  //      if (!expenseToUpdate.expenseLines.Any(el => el.expenseLineId == existingLine.expenseLineId)) {
  //        expenseDb.ExpenseLines.Remove(existingLine);
  //      }
  //    }

  //    // Update and add any expense lines
  //    foreach (ExpenseLine lineToProcess in expenseToUpdate.expenseLines) {
  //      ExpenseLine existingLine =
  //        existingExpense.expenseLines
  //                        .Where(el => ((el.expenseLineId == lineToProcess.expenseLineId) &&
  //                                      (lineToProcess.expenseLineId != 0)))
  //                        .SingleOrDefault();

  //      if (existingLine != null) {
  //        // Schedule portion already exists so update it
  //        expenseDb.Entry(existingLine).CurrentValues.SetValues(lineToProcess);
  //      }
  //      else {
  //        // Insert new child
  //        ExpenseLine newLine = new ExpenseLine {
  //          expenseId = existingExpense.expenseId,
  //          name = lineToProcess.name,
  //          amount = lineToProcess.amount
  //        };

  //        existingExpense.expenseLines.Add(newLine);
  //      }
  //    }

  //    expenseDb.SaveChanges();
  //  }

  //  public void DeleteExpense(int expenseId) {
  //    Expense toDelete = expenseDb.Expenses.Where(e => e.expenseId == expenseId)
  //                                         .Include(e => e.expenseLines)
  //                                         .SingleOrDefault();

  //    if (toDelete != null) {
  //      expenseDb.Expenses.Remove(toDelete);
  //    }
  //    else {
  //      throw new KeyNotFoundException($"No expense exists with ID: {expenseId.ToString()}");
  //    }

  //    expenseDb.SaveChanges();
  //  }
  //}
}
