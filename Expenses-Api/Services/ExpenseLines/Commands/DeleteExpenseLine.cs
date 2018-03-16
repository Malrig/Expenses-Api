using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.DAL;
using ExpensesApi.Models;

namespace ExpensesApi.Services.ExpenseLines {
  public class DeleteExpenseLine : ICommandHandler<DeleteExpenseLineInfo> {
    private ExpenseContext expenseDb;

    public DeleteExpenseLine(ExpenseContext expenseDb) {
      this.expenseDb = expenseDb;
    }

    public void Handle(DeleteExpenseLineInfo command) {
      ExpenseLine expenseLineToDelete = expenseDb.ExpenseLines
                                                 .Where(e => e.expenseLineId == command.expenseLineId)
                                                 .SingleOrDefault();

      expenseDb.ExpenseLines.Remove(expenseLineToDelete);

      expenseDb.SaveChanges();
    }
  }

  public class DeleteExpenseLineInfo {
    public int expenseLineId { get; set; }

    public DeleteExpenseLineInfo() { }
    public DeleteExpenseLineInfo(int expenseLineId) {
      this.expenseLineId = expenseLineId;
    }
  }
}
