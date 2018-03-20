using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.DAL;
using ExpensesApi.Models;

namespace ExpensesApi.Services.ExpenseLines {
  /// <summary>
  /// Command to delete an expense line item using information
  /// from the DeleteExpenseLineInfo class.
  /// </summary>
  public class DeleteExpenseLine : ICommandHandler<DeleteExpenseLineInfo> {
    private ExpenseContext expenseDb;

    /// <summary>
    /// Constructor pulls in all the required services
    /// </summary>
    /// <param name="expenseDb"></param>
    public DeleteExpenseLine(ExpenseContext expenseDb) {
      this.expenseDb = expenseDb;
    }

    /// <summary>
    /// Handle the command.
    /// </summary>
    /// <param name="command"></param>
    public void Handle(DeleteExpenseLineInfo command) {
      ExpenseLine expenseLineToDelete = expenseDb.ExpenseLines
                                                 .Where(e => e.expenseLineId == command.expenseLineId)
                                                 .SingleOrDefault();

      expenseDb.ExpenseLines.Remove(expenseLineToDelete);

      expenseDb.SaveChanges();
    }
  }

  /// <summary>
  /// Class containing all the information required to delete
  /// an expense line.
  /// </summary>
  public class DeleteExpenseLineInfo {
    /// <summary>
    /// ID of the expense line
    /// </summary>  
    public int expenseLineId { get; set; }

    /// <summary>
    /// Default constructor required for EntityFramework
    /// </summary>
    public DeleteExpenseLineInfo() { }
    /// <summary>
    /// Constructor which sets the expense line ID
    /// </summary>
    /// <param name="expenseLineId"></param>    
    public DeleteExpenseLineInfo(int expenseLineId) {
      this.expenseLineId = expenseLineId;
    }
  }
}
