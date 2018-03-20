using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.DAL;
using ExpensesApi.Models;

namespace ExpensesApi.Services.Expenses {
  /// <summary>
  /// Command to delete an expense item using information
  /// from the DeleteExpenseInfo class.
  /// </summary>
  public class DeleteExpense : ICommandHandler<DeleteExpenseInfo> {
    private ExpenseContext expenseDb;

    /// <summary>
    /// Constructor pulls in all the required services
    /// </summary>
    /// <param name="expenseDb"></param>
    public DeleteExpense(ExpenseContext expenseDb) {
      this.expenseDb = expenseDb;
    }

    /// <summary>
    /// Handle the command.
    /// </summary>
    /// <param name="command"></param>
    // TODO - Make this use the expense line delete command
    public void Handle(DeleteExpenseInfo command) {
      Expense expenseToDelete = expenseDb.Expenses
                                         .Where(e => e.expenseId == command.expenseId)
                                         .Include(e => e.expenseLines)
                                         .SingleOrDefault();

      expenseDb.Expenses.Remove(expenseToDelete);

      expenseDb.SaveChanges();
    }
  }
  
  /// <summary>
  /// Class containing all the information required to delete
  /// an expense.
  /// </summary>
  public class DeleteExpenseInfo {
    /// <summary>
    /// ID of the expense
    /// </summary>  
    public int expenseId { get; set; }

    /// <summary>
    /// Default constructor required for EntityFramework
    /// </summary>
    public DeleteExpenseInfo() { }
    /// <summary>
    /// Constructor which sets the expense ID
    /// </summary>
    /// <param name="expenseId"></param>    
    public DeleteExpenseInfo(int expenseId) {
      this.expenseId = expenseId;
    }
  }
}
