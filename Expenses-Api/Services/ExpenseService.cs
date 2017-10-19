using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

using ExpensesApi.Models;
using ExpensesApi.ViewModels;
using ExpensesApi.DAL;

namespace ExpensesApi.Services
{
  // Define the Interface which can be used to interact with Expenses
  public interface IExpenseService {
    IEnumerable<Expense> ListExpenses();
    Expense GetExpense(int expenseId);
    bool CreateExpense(Expense expenseToCreate);
    bool UpdateExpense(int expenseId, Expense newExpenseValues);
  }
  
  // TODO - Add logging to this to capture error messages etc.
  public class ExpenseService : IExpenseService {
    private ModelStateDictionary modelState;
    private ExpenseContext expenseDb;

    public ExpenseService(ModelStateDictionary modelState, ExpenseContext expenseDb) {
      this.modelState = modelState;
      this.expenseDb = expenseDb;
    }

    protected bool ValidateExpense(Expense expenseToValidate) {
      if ((expenseToValidate.name == null) ||
          (expenseToValidate.name.Trim().Length == 0))
        modelState.AddModelError("Name", "Name is required.");
      if ((expenseToValidate.billedDate == null) || 
          (expenseToValidate.billedDate == DateTime.MinValue))
        modelState.AddModelError("billedDate", "A billed date is required.");
      if ((expenseToValidate.expenseLines == null) ||
          (expenseToValidate.expenseLines.Count == 0))
        modelState.AddModelError("expenseLines", "An expense must have at least one expense line.");
      return modelState.IsValid;
    }

    public IEnumerable<Expense> ListExpenses() {
      // TODO - Return this as a ViewModel instead of the direct models.
      return expenseDb.Expenses
                      .Include(e => e.expenseLines);
    }

    public Expense GetExpense(int expenseId){
      // TODO - Return the expense as a viewmodel not hte model directly
      return expenseDb.Expenses.Where(e => e.expenseId == expenseId)
                               .Include(e => e.expenseLines)
                               .SingleOrDefault();
    }

    public bool CreateExpense(Expense expenseToCreate) {
      // Validation logic
      if (!ValidateExpense(expenseToCreate)) {
        return false;
      }

      // Database logic
      try {
        expenseDb.Expenses.Add(expenseToCreate);
        expenseDb.SaveChanges();
      }
      catch {
        return false;
      }
      return true;
    }

    public bool UpdateExpense(int expenseId, Expense expenseToUpdate) {
      // Validation logic
      if (!ValidateExpense(expenseToUpdate)){
        return false;
      }

      // Database logic
      try {
        
        Expense existingExpense = expenseDb.Expenses.Where(e => e.expenseId == expenseId)
                                                    .Include(e => e.expenseLines)
                                                    .SingleOrDefault();
        // Update the expense
        expenseDb.Entry(existingExpense).CurrentValues.SetValues(expenseToUpdate);

        // Delete any expense lines which are no longer required
        foreach (ExpenseLine existingLine in existingExpense.expenseLines) {
          if (!expenseToUpdate.expenseLines.Any(el => el.expenseLineId == existingLine.expenseLineId)) {
            expenseDb.ExpenseLines.Remove(existingLine);
          }
        }

        // Update and add any expense lines
        foreach (ExpenseLine lineToProcess in expenseToUpdate.expenseLines) {
          ExpenseLine existingLine =
            existingExpense.expenseLines
                           .Where(el => ((el.expenseLineId == lineToProcess.expenseLineId) &&
                                         (lineToProcess.expenseLineId != 0)))
                           .SingleOrDefault();

          if (existingLine != null) {
            // Schedule portion already exists so update it
            expenseDb.Entry(existingLine).CurrentValues.SetValues(lineToProcess);
          }
          else {
            // Insert new child
            ExpenseLine newLine = new ExpenseLine {
              expenseId = existingExpense.expenseId,
              name = lineToProcess.name,
              amount = lineToProcess.amount
            };

            existingExpense.expenseLines.Add(newLine);
          }
        }

        expenseDb.SaveChanges();

      }
      catch {
        return false;
      }
      return true;
    }
  }
}
