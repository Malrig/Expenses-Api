using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using ExpensesApi.DAL;
using ExpensesApi.Models;
using ExpensesApi.Services.ExpenseLines;

namespace ExpensesApi.Services.Expenses {
  /// <summary>
  /// Command to add or update a given expense using the information
  /// provided in the AddUpdateExpenseInfo class.
  /// </summary>
  public class AddUpdateExpense : ICommandHandler<AddUpdateExpenseInfo> {
    private ExpenseContext expenseDb;
    private ICommandHandler<AddUpdateExpenseLineInfo> addUpdateExpenseLine;
    private ICommandHandler<DeleteExpenseLineInfo> deleteExpenseLine;

    /// <summary>
    /// Constructor pulls in all the required services
    /// </summary>
    /// <param name="expenseDb"></param>
    /// <param name="addUpdateExpenseLine"></param>
    /// <param name="deleteExpenseLine"></param>
    public AddUpdateExpense(ExpenseContext expenseDb,
                            ICommandHandler<AddUpdateExpenseLineInfo> addUpdateExpenseLine,
                            ICommandHandler<DeleteExpenseLineInfo> deleteExpenseLine) {
      this.expenseDb = expenseDb;
      this.addUpdateExpenseLine = addUpdateExpenseLine;
      this.deleteExpenseLine = deleteExpenseLine;
    }

    /// <summary>
    /// This handles the command making all hte required changes
    /// to the database.
    /// If the expense lines are being updated then call the 
    /// appropriate commands for them.
    /// </summary>
    /// <param name="command"></param>
    public void Handle(AddUpdateExpenseInfo command) {
      Expense existing = expenseDb.Expenses
                                  .Where(e => e.expenseId == command.expenseId)
                                  .Include(e => e.expenseLines)
                                  .SingleOrDefault();

      if (existing == null) {
        expenseDb.Expenses.Add(command.GetExpense());
      }
      else {
        expenseDb.Entry(existing).CurrentValues.SetValues(command);

        // Only update the expense lines if they are specified
        if (command.UpdateExpenseLines()) {
          List<int> linesToDelete = new List<int>();

          // Check expense lines and delete any that no longer exist
          foreach (ExpenseLine existingLine in existing.expenseLines) {
            if (!command.expenseLines.Any(el => el.expenseLineId == existingLine.expenseLineId)) {
              linesToDelete.Add(existingLine.expenseLineId);
            }
          }
          foreach (int expenseLineId in linesToDelete) {
            deleteExpenseLine.Handle(new DeleteExpenseLineInfo(expenseLineId));
          }

          // Update and add any expense lines
          foreach (ExpenseLine lineToProcess in command.expenseLines) {
            addUpdateExpenseLine.Handle(new AddUpdateExpenseLineInfo(lineToProcess));
          }
        }
      }

      expenseDb.SaveChanges();
    }
  }

  /// <summary>
  /// Class containing all the information required to add
  /// or update an expense.
  /// </summary>
  public class AddUpdateExpenseInfo {
    /// <summary>
    /// ID of the expense
    /// </summary>  
    public int expenseId { get; set; }
    /// <summary>
    /// Expense name
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// Date the expense was paid
    /// </summary>
    public DateTime billedDate { get; set; }
    /// <summary>
    /// Date the expense should be recorded against
    /// </summary>
    public DateTime? effectiveDate { get; set; }

    /// <summary>
    /// The expense lines the expense contains
    /// </summary>
    public List<ExpenseLine> expenseLines { get; set; }

    /// <summary>
    /// Whether or not the expense lines for the expense
    /// should be updated.
    /// </summary>
    public bool expenseLinesIncluded { get; set; } = false;

    /// <summary>
    /// Default constructor required for EntityFramework
    /// </summary>
    public AddUpdateExpenseInfo() { }

    /// <summary>
    /// Function which returns the expense item, used for
    /// adding it to the database.
    /// </summary>
    public Expense GetExpense() {
      return new Expense() {
        expenseId = expenseId,
        name = name,
        billedDate = billedDate,
        effectiveDate = effectiveDate,
        expenseLines = expenseLines
      };
    }

    /// <summary>
    /// Function which determines whether the expense lines
    /// should be updated or not.
    /// </summary>
    /// <returns></returns>
    public bool UpdateExpenseLines() {
      return expenseLinesIncluded && expenseLines != null;
    }
  }
}
