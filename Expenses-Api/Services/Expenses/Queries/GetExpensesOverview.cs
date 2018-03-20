using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using ExpensesApi.DAL;
using ExpensesApi.Models;
using ExpensesApi.ViewModels;

namespace ExpensesApi.Services.Expenses {
  /// <summary>
  /// Query to return an overview of all expense items
  /// </summary>
  public class GetExpensesOverview : IQueryHandler<FindAllExpenses, ExpensesOverview> {
    private ExpenseContext expenseDb;

    /// <summary>
    /// Constructor pulls in all the required services
    /// </summary>
    /// <param name="expenseDb"></param>
    public GetExpensesOverview(ExpenseContext expenseDb) {
      this.expenseDb = expenseDb;
    }

    /// <summary>
    /// Actually perform the query.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>

    public ExpensesOverview Handle(FindAllExpenses query) {
      List<Expense> expenses = expenseDb.Expenses
                                        .Include(e => e.expenseLines)
                                        .ToList();

      return new ExpensesOverview(expenses);
    }
  }

  /// <summary>
  /// The class which contains all the information 
  /// required to run the above query
  /// </summary>
  public class FindAllExpenses : IQuery<ExpensesOverview> {
    /// <summary>
    /// Default constructor
    /// </summary>
    public FindAllExpenses() {
    }
  }
}
