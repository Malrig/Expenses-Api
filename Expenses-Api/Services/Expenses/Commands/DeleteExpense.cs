using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.DAL;
using ExpensesApi.Models;

namespace ExpensesApi.Services.Expenses {
  public class DeleteExpense : ICommandHandler<DeleteExpenseInfo> {
    private ExpenseContext expenseDb;

    public DeleteExpense(ExpenseContext expenseDb) {
      this.expenseDb = expenseDb;
    }

    public void Handle(DeleteExpenseInfo command) {
      Expense expenseToDelete = expenseDb.Expenses
                                         .Where(e => e.expenseId == command.expenseId)
                                         .Include(e => e.expenseLines)
                                         .SingleOrDefault();

      expenseDb.Expenses.Remove(expenseToDelete);

      expenseDb.SaveChanges();
    }
  }

  public class DeleteExpenseInfo {
    public int expenseId { get; set; }

    public DeleteExpenseInfo() { }
    public DeleteExpenseInfo(int id) {
      this.expenseId = expenseId;
    }
  }
}
