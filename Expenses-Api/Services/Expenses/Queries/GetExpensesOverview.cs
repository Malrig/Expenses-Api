using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using ExpensesApi.DAL;
using ExpensesApi.Models;
using ExpensesApi.ViewModels;

namespace ExpensesApi.Services.Expenses
{
  public class GetExpensesOverview : IQueryHandler<FindAllExpenses, ExpensesOverview> {
    private ExpenseContext expenseDb;

    public GetExpensesOverview(ExpenseContext expenseDb) {
      this.expenseDb = expenseDb;
    }

    public ExpensesOverview Handle(FindAllExpenses query) {
      List<Expense> expenses = expenseDb.Expenses
                                        .Include(e => e.expenseLines)
                                        .ToList();

      return new ExpensesOverview(expenses);
    }
  }

  public class FindAllExpenses : IQuery<ExpensesOverview> {
    public FindAllExpenses() {
    }
  }
}
